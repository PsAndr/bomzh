#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Engine
{
    [CustomEditor(typeof(Global_control))]
    public class GlobalControlEditor : Editor
    {
        SerializedProperty typeGlobalControl;
        SerializedProperty sceneNow;
        SerializedProperty sceneStart;
        SerializedProperty localization;

        private void OnEnable()
        {
            typeGlobalControl = serializedObject.FindProperty("typeGlobalControl");
            sceneNow = serializedObject.FindProperty("sceneNow");
            sceneStart = serializedObject.FindProperty("sceneStart");
            localization = serializedObject.FindProperty("localization");

            Global_control global_Control = (Global_control)target;
            global_Control.LoadStartSceneValues();
        }

        private void OnDisable()
        {
            Global_control global_Control = (Global_control)target;
            global_Control.SaveStartSceneValues();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Global_control global_Control = (Global_control)target;

            if (global_Control.typeGlobalControl == Global_control.TypeGlobalControl.Game)
            {
                EditorGUILayout.PropertyField(typeGlobalControl);
                EditorGUILayout.Space(20);
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
                base.OnInspectorGUI();
            }
            else
            {
                EditorGUILayout.PropertyField(typeGlobalControl);
                EditorGUILayout.Space(20);
                EditorGUILayout.PropertyField(sceneNow);
                EditorGUILayout.PropertyField(localization);
                EditorGUILayout.PropertyField(sceneStart);
            }

            EditorGUILayout.Space(20);

            if (GUILayout.Button("Update start scene values"))
            {
                global_Control.SaveStartSceneValues();
            }

            EditorGUILayout.Space(2);

            if (GUILayout.Button("Update files"))
            {
                global_Control.UpdateFiles();
            }

            EditorGUILayout.Space(2);

            if (GUILayout.Button("Set default scene now"))
            {
                global_Control.SetSceneNowValuesToStartScene();
            }

            EditorGUILayout.Space(2);

            if (GUILayout.Button("Set default is command show"))
            {
                global_Control.ChangeToDefaultIsCommandShow();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif