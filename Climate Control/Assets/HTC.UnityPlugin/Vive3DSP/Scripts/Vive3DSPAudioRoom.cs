//====================== Copyright 2016-2018, HTC.Corporation. All rights reserved. ======================
using UnityEngine;
using System;
using System.IO;

namespace HTC.UnityPlugin.Vive3DSP
{
    [AddComponentMenu("VIVE/3DSP_AudioRoom")]

    public class Vive3DSPAudioRoom : MonoBehaviour
    {
        public bool RoomEffect
        {
            set { roomEffect = value; }
            get { return roomEffect; }
        }
        [SerializeField]
        private bool roomEffect = true;
        public RoomReverbPreset ReverbPreset
        {
            set { reverbPreset = value; }
            get { return reverbPreset; }
        }
        [SerializeField]
        private RoomReverbPreset reverbPreset = RoomReverbPreset.Generic;
        public RoomBackgroundAudioType BackgroundType
        {
            set
            {
                m_backgroundType = value;
                backgroundType = value;
                BackgroundClip = GetBackgroundAudioClip();
            }
            get { return m_backgroundType; }
        }
        [SerializeField]
        private RoomBackgroundAudioType backgroundType = RoomBackgroundAudioType.None;
        private RoomBackgroundAudioType m_backgroundType = RoomBackgroundAudioType.None;

        public IntPtr RoomObject
        {
            get { return roomObject; }
            set { roomObject = value; }
        }
        private IntPtr roomObject = IntPtr.Zero;

        [System.Obsolete("This will be deprecated. Please use BackgroundVolume instead.")]
        public float backgroundVolume
        {
            get { return m_sourceVolume; }
            set
            {
                m_sourceVolume = value;
                if (audioSource != null)
                {
                    audioSource.volume = (float)Math.Pow(10.0, m_sourceVolume * 0.05);
                }
            }
        }

        public float BackgroundVolume
        {
            get { return m_sourceVolume; }
            set
            {
                m_sourceVolume = value;
                sourceVolume = value;
                if (audioSource != null)
                {
                    audioSource.volume = (float)Math.Pow(10.0, m_sourceVolume * 0.05);
                }
            }
        }
        [SerializeField]
        private float sourceVolume = -30.0f;
        private float m_sourceVolume = -30.0f;
        public Vector3 Size
        {
            set { size = value; }
            get { return size; }
        }
        [SerializeField]
        private Vector3 size = Vector3.one;

        public RoomPlateMaterial Ceiling
        {
            set { ceiling = value; }
            get { return ceiling; }
        }
        [SerializeField]
        private RoomPlateMaterial ceiling = RoomPlateMaterial.Concrete;
        public RoomPlateMaterial FrontWall
        {
            set { frontWall = value; }
            get { return frontWall; }
        }
        [SerializeField]
        private RoomPlateMaterial frontWall = RoomPlateMaterial.Wood;
        public RoomPlateMaterial BackWall
        {
            set { backWall = value; }
            get { return backWall; }
        }
        [SerializeField]
        private RoomPlateMaterial backWall = RoomPlateMaterial.Wood;
        public RoomPlateMaterial RightWall
        {
            set { rightWall = value; }
            get { return rightWall; }
        }
        [SerializeField]
        private RoomPlateMaterial rightWall = RoomPlateMaterial.Carpet;
        public RoomPlateMaterial LeftWall
        {
            set { leftWall = value; }
            get { return leftWall; }
        }
        [SerializeField]
        private RoomPlateMaterial leftWall = RoomPlateMaterial.Wood;
        public RoomPlateMaterial Floor
        {
            set { floor = value; }
            get { return floor; }
        }
        [SerializeField]
        private RoomPlateMaterial floor = RoomPlateMaterial.Concrete;
        public float CeilingReflectionRate
        {
            set { ceilingReflectionRate = value; }
            get { return ceilingReflectionRate; }
        }
        [SerializeField]
        private float ceilingReflectionRate = 1.0f;
        public float FrontWallReflectionRate
        {
            set { frontWallReflectionRate = value; }
            get { return frontWallReflectionRate; }
        }
        [SerializeField]
        private float frontWallReflectionRate = 1.0f;
        public float BackWallReflectionRate
        {
            set { backWallReflectionRate = value; }
            get { return backWallReflectionRate; }
        }
        [SerializeField]
        private float backWallReflectionRate = 1.0f;
        public float RightWallReflectionRate
        {
            set { rightWallReflectionRate = value; }
            get { return rightWallReflectionRate; }
        }
        [SerializeField]
        private float rightWallReflectionRate = 1.0f;
        public float LeftWallReflectionRate
        {
            set { leftWallReflectionRate = value; }
            get { return leftWallReflectionRate; }
        }
        [SerializeField]
        private float leftWallReflectionRate = 1.0f;
        public float FloorReflectionRate
        {
            set { floorReflectionRate = value; }
            get { return floorReflectionRate; }
        }
        [SerializeField]
        private float floorReflectionRate = 1.0f;
        public float ReflectionLevel
        {
            set { reflectionLevel = value; }
            get { return reflectionLevel; }
        }
        [SerializeField]
        public float reflectionLevel = 0.0f;
        public float ReverbLevel
        {
            set { reverbLevel = value; }
            get { return reverbLevel; }
        }
        [SerializeField]
        public float reverbLevel = 0.0f;

        [SerializeField]
        private AudioClip userDefineClip = null;
        
        private AudioSource audioSource = null;
        private AudioClip sourceClip = null;

        public bool isCurrentRoom = false;
        public bool isListenerInsideRoomUpdated = false;

        public VIVE_3DSP_ROOM_PROPERTY RoomPorperty
        {
            get { return roomProperty; }
        }
        private VIVE_3DSP_ROOM_PROPERTY roomProperty;

        [System.Obsolete("This will be deprecated. Please use BackgroundClip instead.")]
        public AudioClip backgroundClip
        {
            get { return sourceClip; }
            set
            {
                if (sourceClip != value)
                {
                    sourceClip = value;
                    if (audioSource != null)
                    {
                        audioSource.clip = sourceClip;
                        PlayBackgroundAudio();
                    }
                }
            }
        }

        private AudioClip BackgroundClip
        {
            get { return sourceClip; }
            set {
                if (sourceClip != value)
                {
                    sourceClip = value;
                    if (audioSource != null)
                    {
                        audioSource.clip = sourceClip;
                        PlayBackgroundAudio();
                    }
                }
            }
        }
        
        public bool isPlaying {
            get {
                if (audioSource != null) { return audioSource.isPlaying; }
                return false;
            }
        }
        
        void Awake()
        {
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.hideFlags = HideFlags.HideInInspector | HideFlags.HideAndDontSave;
            }
            audioSource.spatialize = false;
            audioSource.playOnAwake = true;
            audioSource.loop = true;
            audioSource.dopplerLevel = 0.0f;
            audioSource.spatialBlend = 0.0f;
            
        }
        
        void Start()
        {
            InitRoom();
            BackgroundClip = GetBackgroundAudioClip();
            BackgroundVolume = sourceVolume;
        }

        private void OnEnable()
        {
            RoomEffect = true;
            Update();
        }

        private void OnDisable()
        {
            RoomEffect = false;
            UpdateRoomProperty();
        }

        void Update()
        {
            Vive3DSPAudio.CheckIfListenerInRoom(this);
            if (backgroundType == RoomBackgroundAudioType.UserDefine)
            {
                BackgroundClip = GetBackgroundAudioClip();
            }
            UpdateRoomProperty();
        }

        void OnDestroy()
        {
            Vive3DSPAudio.DestroyRoom(this);
            roomObject = IntPtr.Zero;
            Destroy(audioSource);
        }

        void UpdateRoomProperty()
        {
            roomProperty.position = transform.position;
            roomProperty.rotation = transform.rotation;
            roomProperty.preset = reverbPreset;
            roomProperty.reflection_level = reflectionLevel;
            roomProperty.reverb_level = reverbLevel;
            roomProperty.dry_level = 1.0f;
            roomProperty.gain = 1.0f;
            roomProperty.reflection_rate_left = leftWallReflectionRate;
            roomProperty.reflection_rate_right = rightWallReflectionRate;
            roomProperty.reflection_rate_back = backWallReflectionRate;
            roomProperty.reflection_rate_front = frontWallReflectionRate;
            roomProperty.reflection_rate_ceiling = ceilingReflectionRate;
            roomProperty.reflection_rate_floor = floorReflectionRate;
            roomProperty.material_left = leftWall;
            roomProperty.material_right = rightWall;
            roomProperty.material_back = backWall;
            roomProperty.material_front = frontWall;
            roomProperty.material_ceiling = ceiling;
            roomProperty.material_floor = floor;
            roomProperty.size = Vector3.Scale(transform.lossyScale, size);
            if (this != null && roomObject != IntPtr.Zero)
                Vive3DSPAudio.UpdateRoom(this);

            if (backgroundType != BackgroundType)
            {
                BackgroundType = backgroundType;
            }

            if (sourceVolume != BackgroundVolume)
            {
                BackgroundVolume = sourceVolume;
            }

            if (roomEffect) {
                PlayBackgroundAudio();
            }
            else
                StopBackgroundAudio();
        }

        private void InitRoom()
        {
            Vive3DSPAudio.CreateRoom(this);
        }

        private AudioClip GetBackgroundAudioClip()
        {
            AudioClip tempClip;
            switch (backgroundType)
            {
                case RoomBackgroundAudioType.UserDefine:
                    tempClip = userDefineClip;
                    break;
                case RoomBackgroundAudioType.None:
                    tempClip = null;
                    break;
                default:
                    float[] data = Vive3DSPAudio.GetBGAudioData((int)backgroundType);
                    if (data == null) { 
                        tempClip = null;
                        Debug.Log("clip is null!!!!!!");
                    }
                    else
                    {
                        tempClip = AudioClip.Create("BG Preset", 48000, 1, 48000, false);
                        tempClip.SetData(data, 0);
                    }
                    
                    break;
            }

            return tempClip;
        }

        public void PlayBackgroundAudio()
        {
            if ((audioSource != null) && (!isPlaying) && (backgroundType != RoomBackgroundAudioType.None) && (isCurrentRoom == true) && roomEffect)
            {
                audioSource.Play();
            }
        }

        public void StopBackgroundAudio()
        {
            if ((audioSource != null) && (isPlaying)) {
                audioSource.Stop();
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, size);
        }
    }
}
