#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace Engine
{
    [CustomEditor(typeof(SaveWindow))]
    public class SaveWindowEditor : Editor
    {
        SerializedProperty prefabModule;
        SerializedProperty countLines;
        SerializedProperty countInLine;
        SerializedProperty global_Control;
        SerializedProperty toSpawnModules;
        SerializedProperty deltaHorizontal;
        SerializedProperty scaleWithSavesCount;

        private void OnEnable()
        {
            this.prefabModule = serializedObject.FindProperty("prefabModule");
            this.countLines = serializedObject.FindProperty("countLines");
            this.countInLine = serializedObject.FindProperty("countInLine");
            this.global_Control = serializedObject.FindProperty("global_Control");
            this.toSpawnModules = serializedObject.FindProperty("toSpawnModules");
            this.deltaHorizontal = serializedObject.FindProperty("deltaHorizontal");
            this.scaleWithSavesCount = serializedObject.FindProperty("scaleWithSavesCount");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(this.global_Control);
            EditorGUILayout.PropertyField(this.prefabModule);
            EditorGUILayout.PropertyField(this.toSpawnModules);
            EditorGUILayout.PropertyField(this.countLines);
            EditorGUILayout.PropertyField(this.countInLine);
            EditorGUILayout.PropertyField(this.scaleWithSavesCount);
            EditorGUILayout.PropertyField(this.deltaHorizontal);

            if (((GameObject)this.prefabModule.objectReferenceValue) != null && ((GameObject)this.prefabModule.objectReferenceValue).GetComponent<SaveModule>() == null)
            {
                ((GameObject)this.prefabModule.objectReferenceValue).AddComponent<SaveModule>();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif