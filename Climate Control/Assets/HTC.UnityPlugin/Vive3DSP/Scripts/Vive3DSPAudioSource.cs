//====================== Copyright 2016-2018, HTC.Corporation. All rights reserved. ======================
using UnityEngine;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace HTC.UnityPlugin.Vive3DSP
{
    [AddComponentMenu("VIVE/3DSP_AudioSource")]

    public class Vive3DSPAudioSource : MonoBehaviour
    {
        public AudioSource audioSource
        {
            get { return source; }
            set { source = value; }
        }
        private AudioSource source;

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

        [System.Obsolete("This will be deprecated. Please use SoundDecayEffectSwitch instead.")]
        public bool distanceModeSwitch
        {
            get
            {
                return soundDecayEffectSwitch;
            }
            set
            {
                soundDecayEffectSwitch = value;
            }
        }

        public bool SoundDecayEffectSwitch
        {
            get
            {
                return soundDecayEffectSwitch;
            }
            set
            {
                soundDecayEffectSwitch = value;
            }
        }
        [SerializeField]
        private bool soundDecayEffectSwitch = false;

        public float Gain
        {
            set { gain = value; }
            get { return gain; }
        }
        [SerializeField]
        private float gain = 0.0f;

        [System.Obsolete("This will be deprecated. Please use SoundDecayMode instead.")]
        public Ambisonic3dDistanceMode DistanceMode
        {
            set { soundDecayMode = (SoundDecayMode)value; }
            get { return (Ambisonic3dDistanceMode)soundDecayMode; }
        }

        public SoundDecayMode SoundDecayMode
        {
            set { soundDecayMode = value; }
            get { return soundDecayMode; }
        }
        [SerializeField]
        private SoundDecayMode soundDecayMode = SoundDecayMode.RealWorldDecay;

        public ReverbMode ReverbMode
        {
            set { reverbMode = value; }
            get { return reverbMode; }
        }
        [SerializeField]
        private ReverbMode reverbMode = ReverbMode.Mono;

        public BinauralEngine BinauralEngine
        {
            set { binauralEngine = value; }
            get { return binauralEngine; }
        }
        [SerializeField]
        private BinauralEngine binauralEngine = BinauralEngine.SpeedUp;

        public float MinimumDecayVolumeDb
        {
            set { minimumDecayVolumeDb = value; }
            get { return minimumDecayVolumeDb; }
        }
        [SerializeField]
        private float minimumDecayVolumeDb = -96.0f;

        public float MinDistance
        {
            set { minDistance = Math.Max(value, 0.0f); }
            get { return minDistance; }
        }
        [SerializeField]
        private float minDistance = 0.0f;

        public float MaxDistance
        {
            set { maxDistance = Math.Max(value, 0.0f); }
            get { return maxDistance; }
        }
        [SerializeField]
        private float maxDistance = 1000.0f;

        public float Drc
        {
            set {
                if (value > 0.5)
                    drc = true;
                else
                    drc = false;
            }
            get {
                if (drc)
                    return 1.0f;
                else
                    return 0.0f;
            }
        }
        [SerializeField]
        private bool drc = true;

        [SerializeField]
        private float[] graphicEQGain = new float[20];

        [SerializeField]
        private float[] paramEQGain = Enumerable.Repeat(0.0f, 7).ToArray();
        [SerializeField]
        private float[] paramEQFreq = Enumerable.Repeat(1000.0f, 7).ToArray();
        [SerializeField]
        private float[] paramEQ_Q = Enumerable.Repeat(1.0f, 5).ToArray();

        public float Spatializer3d
        {
            set
            {
                if (value > 0.5)
                    spatializer_3d = true;
                else
                    spatializer_3d = false;
            }
            get
            {
                if (spatializer_3d)
                    return 1.0f;
                else
                    return 0.0f;
            }
        }
        [SerializeField]
        private bool spatializer_3d = true;

        public float Reverb
        {
            set
            {
                if (value > 0.5)
                    reverb = true;
                else
                    reverb = false;
            }
            get
            {
                if (reverb)
                    return 1.0f;
                else
                    return 0.0f;
            }
        }
        [SerializeField]
        private bool reverb = true;

        public float GraphicEQ
        {
            set
            {
                if (value > 0.5f)
                    graphicEQ = true;
                else
                    graphicEQ = false;
            }
            get
            {
                if (graphicEQ)
                    return 1.0f;
                else
                    return 0.0f;
            }
        }
        [SerializeField]
        private bool graphicEQ = false;

        public float ParamEQ
        {
            set
            {
                if (value > 0.5f)
                    paramEQ = true;
                else
                    paramEQ = false;
            }
            get
            {
                if (paramEQ)
                    return 1.0f;
                else
                    return 0.0f;
            }
        }
        [SerializeField]
        private bool paramEQ = false;

        [SerializeField]
        public bool[] ParamEQ_Band_Enable = Enumerable.Repeat(true, 7).ToArray();

        public float Occlusion
        {
            set
            {
                if (value > 0.5)
                    occlusion = true;
                else
                    occlusion = false;
            }
            get
            {
                if (occlusion)
                    return 1.0f;
                else
                    return 0.0f;
            }
        }
        [SerializeField]
        private bool occlusion = true;

        public bool isPlaying
        {
            get
            {
                if (audioSource != null)
                {
                    return audioSource.isPlaying;
                }
                return false;
            }
        }

        public bool isVirtual
        {
            get {
                if (audioSource != null)
                {
                    return audioSource.isVirtual;
                }
                return true;
            }
        }

        public float BypassSmallSignal
        {
            get
            {
                if (bypassSmallSignal) return 1.0f;
                else return 0.0f;
            }
            set
            {
                if (value > 0.5f) bypassSmallSignal = true;
                else bypassSmallSignal = false;
            }
        }
        [SerializeField]
        private bool bypassSmallSignal = false;

        public float BypassSmallSignalValueDB
        {
            get
            {
                return bypassSmallSignalValueDB;
            }
            set
            {
                bypassSmallSignalValueDB = value;
            }
        }
        [SerializeField]
        private float bypassSmallSignalValueDB = -40.0f;

        public float MuteFarDistantSource
        {
            get
            {
                if (muteFarDistantSource) return 1.0f;
                else return 0.0f;
            }
            set
            {
                if (value > 0.5f) muteFarDistantSource = true;
                else muteFarDistantSource = false;
            }
        }
        [SerializeField]
        private bool muteFarDistantSource = false;

        public float MuteFarDistantSourceRadius
        {
            get
            {
                return muteFarDistantSourceRadius;
            }
            set
            {
                muteFarDistantSourceRadius = value;
            }
        }
        [SerializeField]
        private float muteFarDistantSourceRadius = 500.0f;

        // Native audio spatializer effect data.
        public enum EffectData
        {
            Gain = 0,
            BypassSmallSignal,
            BypassSmallSignalValueDB,
            MuteFarDistantSource,
            MuteFarDistantSourceRadius,
            SoundDecayMode,
            EnableDRC,
            EnableSpatialAudioEffect,
            EnableRoomEffect,
            EnableOcclusionEffect,
            EnableGraphicEQ,
            EnableParamEQ,
            CurrentRoom,
            MonoCoverOcclusion,
            MonoCoverRatio,
            StereoCoverOcclusion,
            StereoCoverRatioL,
            StereoCoverRatioR,
            MinDecayVolumeDB,
            MinDistance,
            MaxDistance,
            GraphicEQGain1,
            GraphicEQGain2,
            GraphicEQGain3,
            GraphicEQGain4,
            GraphicEQGain5,
            GraphicEQGain6,
            GraphicEQGain7,
            GraphicEQGain8,
            GraphicEQGain9,
            GraphicEQGain10,
            GraphicEQGain11,
            GraphicEQGain12,
            GraphicEQGain13,
            GraphicEQGain14,
            GraphicEQGain15,
            GraphicEQGain16,
            GraphicEQGain17,
            GraphicEQGain18,
            GraphicEQGain19,
            GraphicEQGain20,
            ParamEQFreq0,
            ParamEQFreq1,
            ParamEQFreq2,
            ParamEQFreq3,
            ParamEQFreq4,
            ParamEQFreq5,
            ParamEQFreq6,
            ParamEQGain0,
            ParamEQGain1,
            ParamEQGain2,
            ParamEQGain3,
            ParamEQGain4,
            ParamEQGain5,
            ParamEQGain6,
            ParamEQ_Q1,
            ParamEQ_Q2,
            ParamEQ_Q3,
            ParamEQ_Q4,
            ParamEQ_Q5,
            ReverbMode,
            BinauralEngine,
            SourceID
        }
        
        void Awake()
        {
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
            if ((audioSource.spatialize == false) && (audioSource.spatialBlend == 0.0f))
            {
                audioSource.spatialBlend = 1.0f;
            };
        }

        void OnEnable()
        {
            if (audioSource == null) return;

            Vive3DSPAudio.CreateSource(this);
            audioSource.enabled = true;
            InitSource();
            
            if (audioSource.playOnAwake && !isPlaying)
            {
                Play();
            }
            Update();
        }

        void OnDisable()
        {
            if (audioSource != null && isPlaying)
            {
                audioSource.Stop();
            }
            Vive3DSPAudio.DestroySource(this);
        }

        void Start()
        {
            if (audioSource.playOnAwake && !isPlaying)
            {
                Play();
            }
			UpdateTransform();
        }

        void Update()
        {
			UpdateTransform();
			
            if (!isPlaying)
            {
                audioSource.Pause();
            }

            if (soundDecayEffectSwitch)
            {
                audioSource.rolloffMode = AudioRolloffMode.Custom;
                audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff,
                                               AnimationCurve.Linear(audioSource.minDistance, 1.0f,
                                                                     audioSource.maxDistance, 1.0f));
            }
            else
            {
                soundDecayMode = SoundDecayMode.NoDecay;
            }
            
            // Update effect data
            if (audioSource.spatialize)
            {
                audioSource.SetSpatializerFloat((int)EffectData.Gain, gain);
                audioSource.SetSpatializerFloat((int)EffectData.BypassSmallSignal, BypassSmallSignal);
                audioSource.SetSpatializerFloat((int)EffectData.BypassSmallSignalValueDB, BypassSmallSignalValueDB);
                audioSource.SetSpatializerFloat((int)EffectData.MuteFarDistantSource, MuteFarDistantSource);
                audioSource.SetSpatializerFloat((int)EffectData.MuteFarDistantSourceRadius, MuteFarDistantSourceRadius);
                audioSource.SetSpatializerFloat((int)EffectData.SoundDecayMode, (float)soundDecayMode);
                audioSource.SetSpatializerFloat((int)EffectData.EnableDRC, Drc);
                audioSource.SetSpatializerFloat((int)EffectData.EnableSpatialAudioEffect, Spatializer3d);
                audioSource.SetSpatializerFloat((int)EffectData.EnableRoomEffect, Reverb);
                audioSource.SetSpatializerFloat((int)EffectData.EnableOcclusionEffect, Occlusion);
                audioSource.SetSpatializerFloat((int)EffectData.EnableGraphicEQ, GraphicEQ);
                audioSource.SetSpatializerFloat((int)EffectData.EnableParamEQ, ParamEQ);
                audioSource.SetSpatializerFloat((int)EffectData.MinDecayVolumeDB, minimumDecayVolumeDb);
                audioSource.SetSpatializerFloat((int)EffectData.MinDistance, minDistance);
                audioSource.SetSpatializerFloat((int)EffectData.MaxDistance, maxDistance);
                audioSource.SetSpatializerFloat((int)EffectData.ReverbMode, (float)reverbMode);
                audioSource.SetSpatializerFloat((int)EffectData.BinauralEngine, (float)binauralEngine);
            }

            if (GraphicEQ > 0.5f)
                setAllGraphicEqGain();
            if (ParamEQ > 0.5)
                setParamEQ();

        }

        private void UpdateTransform()
        {
            Position = transform.position;
        }

        public void Play()
        {
            if (audioSource != null)
            {
                audioSource.Play();
                InitSource();
            }
            else
            {
                Debug.LogWarning("Audio source not initialized. Audio playback not supported " +
                                  "until after Awake() and OnEnable(). Try calling from Start() instead.");
            }
        }

        public void setGraphicEQGainToDefault()
        {
            for (int idx = 0; idx < 20; idx++)
            {
                graphicEQGain[idx] = 0.0f;
                setGraphicEqGain(idx, 0.0f);
            }
        }

        public void setGraphicEqGain(int idx, float gain)
        {
            if (audioSource == null)
            {
                Awake();
                InitSource();
            }
            if (graphicEQGain[idx] != gain)
                graphicEQGain[idx] = gain;

            audioSource.SetSpatializerFloat((int)(EffectData.GraphicEQGain1) + idx, graphicEQGain[idx]);
        }

        public void setAllGraphicEqGain()
        {
            if (audioSource == null)
            {
                Awake();
                InitSource();
            }
            
            for (int idx = 0; idx < 20; idx++)
            {
                audioSource.SetSpatializerFloat((int)(EffectData.GraphicEQGain1) + idx, graphicEQGain[idx]);
            }
        }

        public void setParamEQGain(int idx, float gain)
        {
            if (audioSource == null)
            {
                Awake();
                InitSource();
            }
            if (paramEQGain[idx] != gain)
                paramEQGain[idx] = gain;
            if (ParamEQ_Band_Enable[idx])
                audioSource.SetSpatializerFloat((int)(EffectData.ParamEQGain0) + idx, paramEQGain[idx]);
            else
                audioSource.SetSpatializerFloat((int)(EffectData.ParamEQGain0) + idx, 0.0f);
        }

        public void setParamEQFreq(int idx, float freq)
        {
            if (audioSource == null)
            {
                Awake();
                InitSource();
            }
            if (paramEQFreq[idx] != freq)
                paramEQFreq[idx] = freq;

            audioSource.SetSpatializerFloat((int)(EffectData.ParamEQFreq0) + idx, paramEQFreq[idx]);
        }

        public void setParamEQ_Q(int idx, float Q)
        {
            if (audioSource == null)
            {
                Awake();
                InitSource();
            }
            if (paramEQ_Q[idx] != Q)
                paramEQ_Q[idx] = Q;

            audioSource.SetSpatializerFloat((int)(EffectData.ParamEQ_Q1) + idx, paramEQ_Q[idx]);
        }

        private void setParamEQ()
        {
            for (int idx = 0; idx < 7; idx++)
            {
                setParamEQGain(idx, paramEQGain[idx]);
                setParamEQFreq(idx, paramEQFreq[idx]);
            }
            for (int idx = 0; idx < 5; idx++)
            {
                setParamEQ_Q(idx, paramEQ_Q[idx]);
            }
        }

        void OnApplicationFocus(bool hasFocus)
        {
            if ((hasFocus) && (audioSource != null))
            {
                audioSource.UnPause();
            }
        }

        private bool InitSource()
        {
            if (audioSource != null)
            {
                audioSource.spatialize = true;
            }
            return true;
        }

        private string paramStr;
        public void SetExportParameter(string str)
        {
            paramStr = str;
        }
        public void ClickToExportData(string srcPath, string targetPath)
        {
            vive_3dsp_source_export_plugin(srcPath, targetPath, paramStr);
        }
        private const string pluginName = "audioplugin_vive3dsp";

        [DllImport(pluginName)]
        private static extern int vive_3dsp_source_export_plugin(string srcPath, string targetPath, string data);
    }
}
