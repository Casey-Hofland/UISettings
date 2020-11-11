﻿using System;
using UnityEditor;
using UnityEngine.Audio;

namespace UISettings.Editor
{
    [CustomEditor(typeof(AudioSetting), true)]
    [CanEditMultipleObjects]
    public class AudioSettingEditor : UnityEditor.Editor
    {
        private SerializedProperty audioMixer;
        private SerializedProperty exposedParameter;

        private void OnEnable()
        {
            audioMixer = serializedObject.FindProperty(nameof(audioMixer));
            exposedParameter = serializedObject.FindProperty(nameof(exposedParameter));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(audioMixer);
            if(audioMixer.objectReferenceValue != null)
            {
                var _audioMixer = (AudioMixer)audioMixer.objectReferenceValue;

                // Exposed Parameters Popup
                var parameters = (Array)_audioMixer.GetType().GetProperty("exposedParameters").GetValue(_audioMixer);
                if(parameters.Length > 0)
                {
                    var names = new string[parameters.Length];
                    for(int i = 0; i < parameters.Length; i++)
                    {
                        var parameter = parameters.GetValue(i);
                        names[i] = (string)parameter.GetType().GetField("name").GetValue(parameter);
                    }

                    var index = Math.Max(Array.IndexOf(names, exposedParameter.stringValue), 0);
                    index = EditorGUILayout.Popup("Parameter", index, names);
                    exposedParameter.stringValue = names[index];

                    // Other Values
                    var property = exposedParameter.Copy();
                    while(property.NextVisible(false))
                    {
                        EditorGUILayout.PropertyField(property, true);
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("The AudioMixer doesn't have any exposed parameters on it.", MessageType.Warning, true);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
