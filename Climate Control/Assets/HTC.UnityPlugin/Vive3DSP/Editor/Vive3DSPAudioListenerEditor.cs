//====================== Copyright 2016-2018, HTC.Corporation. All rights reserved. ======================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HTC.UnityPlugin.Vive3DSP
{
    [CustomEditor(typeof(Vive3DSPAudioListener))]
    public class Vive3DSPAudioListenerEditor : Editor
    {
        private SerializedProperty globalGain = null;
        private SerializedProperty occlusionMode = null;
        private SerializedProperty headset = null;
        private SerializedProperty isRecordToFile = null;
        private SerializedProperty fname = null;


        private GUIContent globalGainLabel = new GUIContent("Global Gain (dB)",
            "Set the global gain of the system");
        private GUIContent occlusionModeLabel = new GUIContent("Occlusion Mode",
            "Set occlusion mode");
        private GUIContent headsetLabel = new GUIContent("Headset Model",
            "Set the headset to compensate");
        private GUIContent isRecordToFileLabel = new GUIContent(
            "Record Audio Listener",
            "Check the box to record");

        private string export_filepath = "";

        void OnEnable()
        {
            globalGain = serializedObject.FindProperty("globalGain");
            occlusionMode = serializedObject.FindProperty("occlusionMode");
            headset = serializedObject.FindProperty("headsetType");
            isRecordToFile = serializedObject.FindProperty("isRecordToFile");
            fname = serializedObject.FindProperty("fname");
        }

        public override void OnInspectorGUI()
        {
            Vive3DSPAudioListener model = target as Vive3DSPAudioListener;
            serializedObject.Update();

            EditorGUILayout.Slider(globalGain, -24.0f, 24.0f, globalGainLabel);
            EditorGUILayout.Separator();

            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(occlusionMode, occlusionModeLabel);
            EditorGUILayout.Separator();

            if (Application.isPlaying)
                GUI.enabled = false;
            else
                GUI.enabled = true;
            EditorGUILayout.PropertyField(headset, headsetLabel);

            GUI.enabled = true;
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(isRecordToFile, isRecordToFileLabel);
            if (GUILayout.Button(
                "Set path", GUILayout.Width(120), GUILayout.Height(20)))
            {
                export_filepath = EditorUtility.SaveFilePanel(
                    "Record to wav file", Application.dataPath, "", "wav");
                model.SetRecordWavFileName(export_filepath);
            }
            if (fname.stringValue == "")
                GUILayout.Label("Path: << Please set the file path. >>");
            else
                GUILayout.Label("Path: " + fname.stringValue);

            serializedObject.ApplyModifiedProperties();
        }
    }
}