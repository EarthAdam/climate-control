//====================== Copyright 2016-2018, HTC.Corporation. All rights reserved. ======================
using UnityEngine;
using UnityEditor;

namespace HTC.UnityPlugin.Vive3DSP
{
    [CustomEditor(typeof(Vive3DSPAudioSource))]
    [CanEditMultipleObjects]
    public class Vive3DSPAudioSourceEditor : Editor
    {
        private SerializedProperty gain = null;
        private SerializedProperty soundDecayEffectSwitch = null;
        private SerializedProperty soundDecayMode = null;
        private SerializedProperty drc = null;
        private SerializedProperty spatializer = null;
        private SerializedProperty reverb = null;
        private SerializedProperty reverbMode = null;
        private SerializedProperty binauralEngine = null;
        private SerializedProperty occlusion = null;
        private SerializedProperty minimumDecayVolumeDb = null;
        private SerializedProperty minDistance = null;
        private SerializedProperty maxDistance = null;
        private SerializedProperty graphicEQ = null;
        private SerializedProperty graphicEQGain = null;
        private SerializedProperty[] graphicEQGainArray = new SerializedProperty[20];
        private SerializedProperty paramEQ = null;
        private SerializedProperty paramEQFreq = null;
        private SerializedProperty paramEQGain = null;
        private SerializedProperty paramEQ_Q = null;
        private SerializedProperty paramEQ_Band_Enable = null;
        private SerializedProperty[] paramEQFreqArray = new SerializedProperty[7];
        private SerializedProperty[] paramEQGainArray = new SerializedProperty[7];
        private SerializedProperty[] paramEQ_QArray = new SerializedProperty[5];
        private SerializedProperty[] paramEQ_Band_Enable_Array = new SerializedProperty[7];
        private SerializedProperty bypassSmallSignal = null;
        private SerializedProperty bypassSmallSignalValueDB = null;
        private SerializedProperty muteFarDistantSource = null;
        private SerializedProperty muteFarDistantSourceRadius = null;

        private GUIContent gainLabel = new GUIContent(
            "Gain (dB)",
            "Set the gain of the sound source");
        private GUIContent soundDecayEffectSwitchLabel = new GUIContent(
            "Overwrite Volume Rolloff",
            "Enable 3DSP sound decay effect to overwrite unity audio source volume rolloff");
        private GUIContent soundDecayModeLabel = new GUIContent(
            "Sound Decay Effect",
            "Set sound decay mode");
        private GUIContent drcLabel = new GUIContent(
            "DRC",
            "Set the dynamic range compression (DRC) feature");
        private GUIContent spatializerLabel = new GUIContent(
            "3D Sound Effect",
            "Set the 3D sound effect feature");
        private GUIContent reverbLabel = new GUIContent(
            "Room Effect",
            "Set the reverb effect feature");
	    private GUIContent reverbModeLabel = new GUIContent(
            "Room Reverb Mode",
            "Set the reverb mode");
        private GUIContent binauralEngineLabel = new GUIContent(
            "Binaural Engine",
            "Set the binaural reverb engine");
        private GUIContent graphicEQLabel = new GUIContent(
            "Graphic Equalizer",
            "Set the graphic equalizer feature");
        private GUIContent paramEQLabel = new GUIContent(
            "Parametric Equalizer",
            "Set the parametric equalizer feature");
        private GUIContent occlusionLabel = new GUIContent(
            "Occlusion Effect",
            "Set the occlusion effect feature");
        private GUIContent minimumDecayVolumeTapDbLabel = new GUIContent(
            "Minimum Decay Volume (dB)",
            "Set minimum decay volume");
        private GUIContent minimumDecayVolumeDbLabel = new GUIContent(
            " ",
            "Set minimum decay volume");
        private GUIContent minDistanceLabel = new GUIContent(
            "Minimum Distance (M)",
            "Set minimum distance");
        private GUIContent maxDistanceLabel = new GUIContent(
            "Maximum Distance (M)",
            "Set maximum distance");
        private GUIContent[] graphicEQGainArrayLabel = new GUIContent[20];
        private GUIContent paramEQFreqLabel = new GUIContent(
            "Frequency",
            "Set center frequency");
        private GUIContent paramEQGainLabel = new GUIContent(
            "Gain",
            "Set gain");
        private GUIContent paramEQ_QLabel = new GUIContent(
            "Q",
            "Set the Q value");
        private GUIContent paramEQLowShelfLabel = new GUIContent(
            "Low Shelving Filter",
            "Enable the low shelving filter");
        private GUIContent paramEQPeakingLabel = new GUIContent(
            "Peaking Filter",
            "Enable the peaking filter");
        private GUIContent paramEQHighShelfLabel = new GUIContent(
            "High Shelving Filter",
            "Enable the high shelving filter");
        private GUIContent bypassSmallSignalLabel = new GUIContent(
            "Bypass Small Signal",
            "Bypass all effects if input signal is small.");
        private GUIContent bypassSmallSignalValueDBLabel = new GUIContent(
            "Gain",
            "Bypass all effects if input signal belows this value.");
        private GUIContent muteFarDistantSourceLabel = new GUIContent(
           "Mute Far Distant Source",
           "Mute source if it is far from listener.");
        private GUIContent muteFarDistantSourceRadiusLabel = new GUIContent(
            "Radius",
            "Mute source if its distance from listener is belows this value.");
        private float[] frequencys = new float[] {
            31.0f, 44.0f, 63.0f, 88.0f, 125.0f,
            180.0f, 250.0f, 355.0f, 500.0f, 710.0f,
            1000.0f, 1400.0f, 2000.0f, 2800.0f, 4000.0f,
            5600.0f, 8000.0f, 11300.0f, 16000.0f, 22000.0f};

        private EqualizerCustomGUI paramEQUI = new EqualizerCustomGUI();

        void OnEnable()
        {
            Vive3DSPAudioSource model = target as Vive3DSPAudioSource;
            gain = serializedObject.FindProperty("gain");
            soundDecayMode = serializedObject.FindProperty("soundDecayMode");
            soundDecayEffectSwitch = serializedObject.FindProperty("soundDecayEffectSwitch");
            drc = serializedObject.FindProperty("drc");
            spatializer = serializedObject.FindProperty("spatializer_3d");
            reverb = serializedObject.FindProperty("reverb");
            reverbMode = serializedObject.FindProperty("reverbMode");
            binauralEngine = serializedObject.FindProperty("binauralEngine");
            occlusion = serializedObject.FindProperty("occlusion");
            minimumDecayVolumeDb = serializedObject.FindProperty("minimumDecayVolumeDb");
            minDistance = serializedObject.FindProperty("minDistance");
            maxDistance = serializedObject.FindProperty("maxDistance");
            graphicEQ = serializedObject.FindProperty("graphicEQ");
            graphicEQGain = serializedObject.FindProperty("graphicEQGain");
            paramEQ = serializedObject.FindProperty("paramEQ");
            bypassSmallSignal = serializedObject.FindProperty("bypassSmallSignal");
            bypassSmallSignalValueDB = serializedObject.FindProperty("bypassSmallSignalValueDB");
            muteFarDistantSource = serializedObject.FindProperty("muteFarDistantSource");
            muteFarDistantSourceRadius = serializedObject.FindProperty("muteFarDistantSourceRadius");

            for (int idx = 0; idx < 20; idx++)
            {
                graphicEQGainArray[idx] = graphicEQGain.GetArrayElementAtIndex(idx);
                if (frequencys[idx] < 1000.0f)
                    graphicEQGainArrayLabel[idx] = new GUIContent((int)frequencys[idx] + " Hz", "");
                else
                    graphicEQGainArrayLabel[idx] = new GUIContent((frequencys[idx] / 1000.0f) + " kHz", "");
                model.setGraphicEqGain(idx, graphicEQGainArray[idx].floatValue);
            }

            paramEQFreq = serializedObject.FindProperty("paramEQFreq");
            paramEQGain = serializedObject.FindProperty("paramEQGain");
            paramEQ_Q = serializedObject.FindProperty("paramEQ_Q");
            paramEQ_Band_Enable = serializedObject.FindProperty("ParamEQ_Band_Enable");

            for (int idx = 0; idx < 7; idx++)
            {
                paramEQFreqArray[idx] = paramEQFreq.GetArrayElementAtIndex(idx);
                paramEQGainArray[idx] = paramEQGain.GetArrayElementAtIndex(idx);
                paramEQ_Band_Enable_Array[idx] = paramEQ_Band_Enable.GetArrayElementAtIndex(idx);
            }

            for (int idx = 0; idx < 5; idx++)
            { 
                paramEQ_QArray[idx] = paramEQ_Q.GetArrayElementAtIndex(idx);
            }
        }

        public override void OnInspectorGUI()
        {
            checkAudioFileType();
            serializedObject.Update();

            EditorGUILayout.Slider(gain, -24.0f, 24.0f, gainLabel);

            EditorGUILayout.PropertyField(bypassSmallSignal, bypassSmallSignalLabel);
            if (bypassSmallSignal.boolValue == true)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Slider(bypassSmallSignalValueDB, -96.0f, -40.0f, bypassSmallSignalValueDBLabel);
                EditorGUILayout.LabelField("dB", GUILayout.Width(50.0f));
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.PropertyField(muteFarDistantSource, muteFarDistantSourceLabel);
            if (muteFarDistantSource.boolValue == true)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Slider(muteFarDistantSourceRadius, 0.0f, 2000.0f, muteFarDistantSourceRadiusLabel);
                EditorGUILayout.LabelField("M", GUILayout.Width(50.0f));
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.PropertyField(drc, drcLabel);
            EditorGUILayout.PropertyField(spatializer, spatializerLabel);
            EditorGUILayout.PropertyField(reverb, reverbLabel);

            drawReverbModeInspector();
            drawSoundDecayEffectInspector();
            EditorGUILayout.Separator();
            drawGraphicEQInspector();
            drawParamEQInspector();
            drawExportWavInspector();

            markSceneDirty();
        }

        private void checkAudioFileType()
        {
            Vive3DSPAudioSource model = target as Vive3DSPAudioSource;
            AudioClip clip = model.audioSource.clip;
            if (clip != null)
            {
                var clipProperty = clip.GetType().GetProperty("ambisonic");
                if (clipProperty != null)
                {
                    if ((bool)clipProperty.GetValue(clip, null))
                    {
                        EditorGUILayout.HelpBox("The audio clip is ambisonic file. Please remove the Vive 3DSP Audio Source and disable spatialize checkbox in audio source.", MessageType.Error);
                        Debug.LogError("The audio clip is ambisonic file. Please remove the Vive 3DSP Audio Source and disable spatialize checkbox in audio source.");
                    }
                }
            }
        }

        private void drawReverbModeInspector()
        {
            EditorGUILayout.PropertyField(reverbMode, reverbModeLabel);
            
            if ((ReverbMode)reverbMode.enumValueIndex == ReverbMode.Binaural)
            {
                ++EditorGUI.indentLevel;
                EditorGUILayout.PropertyField(binauralEngine, binauralEngineLabel);
                --EditorGUI.indentLevel;
            }
        }

        private void drawSoundDecayEffectInspector()
        {
            EditorGUILayout.PropertyField(occlusion, occlusionLabel);
            EditorGUILayout.PropertyField(soundDecayEffectSwitch, soundDecayEffectSwitchLabel);
            
            if (soundDecayEffectSwitch.boolValue == true)
            {
                EditorGUILayout.PropertyField(soundDecayMode, soundDecayModeLabel);
                if (soundDecayMode.enumValueIndex == (int)SoundDecayMode.PointSourceDecay
                    || soundDecayMode.enumValueIndex == (int)SoundDecayMode.LineSourceDecay
                    || soundDecayMode.enumValueIndex == (int)SoundDecayMode.LinearDecay)
                {
                    ++EditorGUI.indentLevel;
                    EditorGUILayout.LabelField(minimumDecayVolumeTapDbLabel);
                    ++EditorGUI.indentLevel;
                    EditorGUILayout.Slider(minimumDecayVolumeDb, -96.0f, 0.0f, minimumDecayVolumeDbLabel);
                    --EditorGUI.indentLevel;
                    --EditorGUI.indentLevel;
                }

                if (soundDecayMode.enumValueIndex == (int)SoundDecayMode.LinearDecay)
                {
                    ++EditorGUI.indentLevel;

                    // Minimum Effective Distance Input Box
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(minDistanceLabel);
                    ++EditorGUI.indentLevel;
                    float min_distance = 0.0f;
                    string input_str = EditorGUILayout.TextField(minDistance.floatValue.ToString("F3"));
                    bool parsed = float.TryParse(input_str, out min_distance);
                    if (!parsed)
                    {
                        minDistance.floatValue = 0.0f;
                        Debug.LogWarning("The input " + input_str + " may not be a float number.");
                    }
                    minDistance.floatValue = min_distance;
                    EditorGUILayout.EndHorizontal();
                    --EditorGUI.indentLevel;

                    // Maximum Effective Distance Input Box
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(maxDistanceLabel);
                    ++EditorGUI.indentLevel;
                    input_str = EditorGUILayout.TextField(maxDistance.floatValue.ToString("F3"));
                    float max_distance = 1000.0f;
                    parsed = float.TryParse(input_str, out max_distance);
                    if (!parsed)
                    {
                        max_distance = 1000.0f;
                        Debug.LogWarning("The input " + input_str + " may not be a float number.");
                    }
                    else if (max_distance < min_distance)
                    {
                        max_distance = min_distance;
                    }
                    maxDistance.floatValue = max_distance;
                    EditorGUILayout.EndHorizontal();
                    --EditorGUI.indentLevel;

                    --EditorGUI.indentLevel;
                }

                switch (soundDecayMode.enumValueIndex)
                {
                    case (int)SoundDecayMode.RealWorldDecay:
                        EditorGUILayout.HelpBox("The sound source changes by the real measured distance filters.", MessageType.Info);
                        break;
                    case (int)SoundDecayMode.PointSourceDecay:
                        EditorGUILayout.HelpBox("The sound source behaves like point source, which the volume decay rate is the inverse of the square of distance.", MessageType.Info);
                        break;
                    case (int)SoundDecayMode.LineSourceDecay:
                        EditorGUILayout.HelpBox("The sound source behaves like line source, which the volume decay rate is the inverse of the distance.", MessageType.Info);
                        break;
                    case (int)SoundDecayMode.LinearDecay:
                        EditorGUILayout.HelpBox("The volume decay rate is 1.0 when the distance is below the minimum effective distance, and it is 0.0 when the distance is above the maximum effective distance. Between them the decay rate is linear decreased.", MessageType.Info);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                EditorGUILayout.HelpBox("To overwirte Audio Source volume rolloff will enable 3DSP Sound Decay Effect", MessageType.Info);
            }
        }

        private void drawGraphicEQInspector()
        {
            EditorGUILayout.PropertyField(graphicEQ, graphicEQLabel);
            
            if (graphicEQ.boolValue == true)
            {
                ++EditorGUI.indentLevel;
                for (int idx = 0; idx < frequencys.Length; idx++)
                {
                    EditorGUILayout.Slider(graphicEQGainArray[idx], -12.0f, 12.0f, graphicEQGainArrayLabel[idx]);
                }

                if (GUILayout.Button("Set to default", GUILayout.Width(100), GUILayout.Height(20)))
                {
                    foreach (Vive3DSPAudioSource model in targets)
                        model.setGraphicEQGainToDefault();
                }
                --EditorGUI.indentLevel;
            }
        }

        private void drawParamEQInspector()
        {
            
            EditorGUILayout.PropertyField(paramEQ, paramEQLabel);
            
            if (paramEQ.boolValue == true)
            {
                ++EditorGUI.indentLevel;
                EditorGUILayout.BeginVertical();
                EditorGUILayout.PropertyField(paramEQ_Band_Enable_Array[0], paramEQLowShelfLabel);
                ++EditorGUI.indentLevel;
                drawParamEQFreqSliderBar(0);
                drawParamEQGainSliderBar(0);
                --EditorGUI.indentLevel;
                for (int idx = 0; idx < 5; idx++)
                {
                    EditorGUILayout.PropertyField(paramEQ_Band_Enable_Array[idx+1], paramEQPeakingLabel);
                    ++EditorGUI.indentLevel;
                    drawParamEQFreqSliderBar(idx + 1);
                    drawParamEQGainSliderBar(idx + 1);
                    drawParamEQ_QSliderBar(idx);
                    --EditorGUI.indentLevel;
                }
                EditorGUILayout.PropertyField(paramEQ_Band_Enable_Array[6], paramEQHighShelfLabel);

                ++EditorGUI.indentLevel;
                drawParamEQFreqSliderBar(6);
                drawParamEQGainSliderBar(6);
                --EditorGUI.indentLevel;
                EditorGUILayout.EndVertical();

                float[] freq = new float[7];
                float[] gain = new float[7];
                float[] Q = new float[5];
                bool[] enable_array = new bool[7];

                for (int i = 0; i < 7; i++)
                {
                    freq[i] = paramEQFreqArray[i].floatValue;
                    gain[i] = paramEQGainArray[i].floatValue;
                    enable_array[i] = paramEQ_Band_Enable_Array[i].boolValue;
                }
                for (int i = 0; i < 5; i++)
                {
                    Q[i] = paramEQ_QArray[i].floatValue;
                }
                paramEQUI.DrawParamEQGUI(freq, gain, Q, enable_array);

                --EditorGUI.indentLevel;
            }
        }

        public void drawExportWavInspector()
        {
            Vive3DSPAudioSource model = target as Vive3DSPAudioSource;
            AudioClip clip = model.audioSource.clip;

            EditorGUILayout.LabelField("Export to wav file");
            if (GUILayout.Button("Export Audio File", GUILayout.Width(150), GUILayout.Height(20)))
            {
                string tmpStr_ = "";

                bool param_eq_switch = (model.ParamEQ > 0.5 ? true : false);
                
                // Gain of all bands of parametric EQ
                for (int idx = 0; idx < paramEQGainArray.Length; idx++)
                {
                    if (param_eq_switch == true && model.ParamEQ_Band_Enable[idx])
                    {
                        tmpStr_ = tmpStr_ + "_" + paramEQGainArray[idx].floatValue.ToString();
                    }
                    else
                    {
                        tmpStr_ = tmpStr_ + "_0.0";
                    }
                }

                // Frequency of all bands of parametric EQ
                for (int idx = 0; idx < paramEQFreqArray.Length; idx++)
                {
                    tmpStr_ = tmpStr_ + "_" + paramEQFreqArray[idx].floatValue.ToString();
                }

                // Q of middle bands of parametric EQ
                for (int idx = 0; idx < paramEQ_QArray.Length; idx++)
                {
                    tmpStr_ = tmpStr_ + "_" + paramEQ_QArray[idx].floatValue.ToString();
                }
                
                bool graphic_eq_switch = (model.GraphicEQ > 0.5 ? true : false);

                // Gain of all bands of graphic EQ
                for (int idx = 0; idx < frequencys.Length; idx++)
                {
                    if (graphic_eq_switch == true)
                    {
                        tmpStr_ = tmpStr_ + "_" + graphicEQGainArray[idx].floatValue.ToString();
                    }
                    else
                    {
                        tmpStr_ = tmpStr_ + "_0.0";
                    }
                }


                // Input gain
                tmpStr_ = tmpStr_ + "_" + gain.floatValue.ToString();

                model.SetExportParameter(tmpStr_);

                var export_filepath = EditorUtility.SaveFilePanel("Export to wav file", Application.dataPath, "", "wav");
                if (export_filepath != "")
                {
                    model.ClickToExportData(get_clip_filepath(clip), export_filepath);
                    AssetDatabase.Refresh();
                }
            }

            EditorGUILayout.Separator();
        }

        string get_clip_filepath(AudioClip clip)
        {
            if (clip == null)
            {
                return "";
            }
            char split_char = '/';
            string[] asset_path = Application.dataPath.Split(split_char);
            if (asset_path == null)
            {
                split_char = '\\';
                asset_path = Application.dataPath.Split(split_char);
            }
            string clip_path = "";
            for (int i = 0; i < asset_path.Length - 1; i++)
            {
                clip_path += asset_path[i] + split_char.ToString();
            }
            clip_path += AssetDatabase.GetAssetPath(clip);
            return clip_path;
        }

        private void drawParamEQFreqSliderBar(int idx)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Slider(paramEQFreqArray[idx], 20.0f, 20000.0f, paramEQFreqLabel);
            EditorGUILayout.LabelField("Hz", GUILayout.Width(50.0f));
            EditorGUILayout.EndHorizontal();
        }

        private void drawParamEQGainSliderBar(int idx)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Slider(paramEQGainArray[idx], -12.0f, 12.0f, paramEQGainLabel);
            EditorGUILayout.LabelField("dB", GUILayout.Width(50.0f));
            EditorGUILayout.EndHorizontal();
        }

        private void drawParamEQ_QSliderBar(int idx)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Slider(paramEQ_QArray[idx], 0.2f, 5.0f, paramEQ_QLabel);
            EditorGUILayout.LabelField("  ", GUILayout.Width(50.0f));
            EditorGUILayout.EndHorizontal();
        }

        private void markSceneDirty()
        {
            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
                if (!Application.isPlaying)
                    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
            }
        }
    }
}


