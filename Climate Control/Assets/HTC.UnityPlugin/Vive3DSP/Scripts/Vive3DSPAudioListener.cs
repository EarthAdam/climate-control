//====================== Copyright 2016-2018, HTC.Corporation. All rights reserved. ======================
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace HTC.UnityPlugin.Vive3DSP
{
    [AddComponentMenu("VIVE/3DSP_AudioListener")]

    public class Vive3DSPAudioListener : MonoBehaviour
    {
        public float globalGain = 0.0f;
        private Vive3DSPAudioRoom currentRoom;
        public Vive3DSPAudioRoom CurrentRoom
        {
            get { return currentRoom; }
            set {
                if (currentRoom != value)
                {
                    if (currentRoom != null)
                    {
                        currentRoom.StopBackgroundAudio();
                        currentRoom.isCurrentRoom = false;
                    }

                    currentRoom = value;

                    if (currentRoom != null)
                    {
                        currentRoom.isCurrentRoom = true;
                        currentRoom.PlayBackgroundAudio();
                    }
                }
            }
        }

        public OccRaycastMode occlusionMode = OccRaycastMode.BinauralOcclusion;
        public HeadsetType headsetType = HeadsetType.Generic;

        public Vector3 Position
        {
            get
            {
                return pos;
            }
            set
            {
                if (pos != value)
                {
                    pos = value;
                }
            }
        }
        private Vector3 pos = Vector3.zero;

        public Quaternion Rotation
        {
            get { return quat; }
            set
            {
                if (quat != value)
                {
                    quat = value;
                }
            }
        }
        private Quaternion quat = Quaternion.identity;

        public HashSet<Vive3DSPAudioRoom> RoomList
        {
            get { return roomList; }
        }
        private HashSet<Vive3DSPAudioRoom> roomList = new HashSet<Vive3DSPAudioRoom>();

        public bool BypassFarDistantSource
        {
            get { return bypassFarDistantSource; }
            set { bypassFarDistantSource = value; }
        }
        [SerializeField]
        private bool bypassFarDistantSource = false;

        void Awake()
        {
            Vive3DSPAudio.CreateAudioListener(this);
            OnValidate();
        }

        void OnEnable()
        {
            UpdateTransform();
        }

        void Update()
        {
            UpdateTransform();

            if (isRecordToFile == true && 
                recordStatus == RecordStatus.STOP &&
                fname != "")
            {
                WavWriter.Create(fname);
                recordStatus = RecordStatus.RECORDING;
            }

            if (isRecordToFile == false &&
                recordStatus == RecordStatus.RECORDING)
            {
                WavWriter.Close();
                recordStatus = RecordStatus.STOP;
            }
        }

        void OnAudioFilterRead(float[] data, int channels)
        {
            if (recordStatus == RecordStatus.RECORDING)
            {
                WavWriter.Write(data);
            }
        }

        private void FixedUpdate()
        {
            Vive3DSPAudio.UpdateOcclusionCoverRatio();
        }

        public void OnValidate()
        {
            Vive3DSPAudio.UpdateAudioListener();
        }

        void OnDisable()
        {
        }

        private void OnDestroy()
        {
            WavWriter.Close();
            Vive3DSPAudio.DestroyAudioListener();
        }
        private void UpdateTransform()
        {
            Position = transform.position;
            Rotation = transform.rotation;
        }

        public enum RecordStatus
        {
            STOP = 0,
            RECORDING
        }
        private RecordStatus recordStatus = RecordStatus.STOP;

        public bool IsRecordToFile
        {
            set { isRecordToFile = value; }
            get { return isRecordToFile; }
        }
        [SerializeField]
        private bool isRecordToFile = false;

        public string RecordFileName
        {
            set { fname = value; }
            get { return fname; }
        }
        [SerializeField]
        private string fname = "";
        public void SetRecordWavFileName(string name)
        {
            fname = name;
        }
    }

    public static class WavWriter
    {
        private const int HEADER_SIZE = 44;
        private static FileStream fs = null;
        private static int SCALE_UP_FACTOR = 32767;

        public static void Create(string fileName)
        {
            fs = new FileStream(fileName, FileMode.Create);
            var emptyByte = new byte();
            for(int i = 0; i < HEADER_SIZE; i++)
            {
                fs.WriteByte(emptyByte);
            }
        }
        public static void Close()
        {
            if (fs != null)
            {
                WriteHeader();
                fs.Close();
                fs = null;

                #if UNITY_EDITOR
                UnityEditor.AssetDatabase.Refresh();
                #endif
            }
        }

        public static void Write(float[] data)
        {
            if (fs != null)
            {
                Int16[] intData = new Int16[data.Length];
                Byte[] byteData = new Byte[data.Length * 2];
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] > 1.0f) data[i] = 1.0f;
                    if (data[i] < -1.0f) data[i] = -1.0f;

                    intData[i] = (short)(data[i] * SCALE_UP_FACTOR);
                    Byte[] byteArr = new Byte[2];
                    byteArr = BitConverter.GetBytes(intData[i]);
                    byteArr.CopyTo(byteData, i * 2);
                }

                fs.Write(byteData, 0, byteData.Length);
            }
        }

        private static void WriteHeader()
        {
            var sample_rate = AudioSettings.outputSampleRate;
            var channel_num = (int)AudioSettings.speakerMode;
            var sample_num = (fs.Length - 44) / channel_num / 2;

            fs.Seek(0, SeekOrigin.Begin);

            Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
            fs.Write(riff, 0, 4);

            Byte[] chunkSize = BitConverter.GetBytes(fs.Length - 8);
            fs.Write(chunkSize, 0, 4);

            Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
            fs.Write(wave, 0, 4);

            Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
            fs.Write(fmt, 0, 4);

            Byte[] subChunk1 = BitConverter.GetBytes(16);
            fs.Write(subChunk1, 0, 4);

            UInt16 one = 1;
            Byte[] audioFormat = BitConverter.GetBytes(one);
            fs.Write(audioFormat, 0, 2);

            Byte[] numChannels = BitConverter.GetBytes(channel_num);
            fs.Write(numChannels, 0, 2);

            Byte[] sampleRate = BitConverter.GetBytes(sample_rate);
            fs.Write(sampleRate, 0, 4);

            Byte[] byteRate = BitConverter.GetBytes(sample_rate * channel_num * 2);
            fs.Write(byteRate, 0, 4);

            UInt16 bloclAlign = (ushort)(channel_num * 2);
            fs.Write(BitConverter.GetBytes(bloclAlign), 0, 2);

            UInt16 bps = 16;
            Byte[] bitPerSample = BitConverter.GetBytes(bps);
            fs.Write(bitPerSample, 0, 2);

            Byte[] dataString = System.Text.Encoding.UTF8.GetBytes("data");
            fs.Write(dataString, 0, 4);

            Byte[] subChunk2 = BitConverter.GetBytes(sample_num * channel_num * 2);
            fs.Write(subChunk2, 0, 4);
        }
    }
}
