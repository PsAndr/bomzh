using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(LoadWindow))]
public class LoadWindowEditor : Editor
{
    SerializedProperty restartScene;
    SerializedProperty global_Control;
    SerializedProperty prefabModule;
    SerializedProperty countLines;
    SerializedProperty deltaHorizontal;
    SerializedProperty toSpawnModules;

    private void OnEnable()
    {
        this.restartScene = serializedObject.FindProperty("restartScene");
        this.global_Control = serializedObject.FindProperty("global_Control");
        this.prefabModule = serializedObject.FindProperty("prefabModule");
        this.countLines = serializedObject.FindProperty("countLines");
        this.deltaHorizontal = serializedObject.FindProperty("deltaHorizontal");
        this.toSpawnModules = serializedObject.FindProperty("toSpawnModules");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(this.global_Control);
        EditorGUILayout.PropertyField(this.prefabModule);
        EditorGUILayout.PropertyField(this.toSpawnModules);
        EditorGUILayout.PropertyField(this.countLines);
        EditorGUILayout.PropertyField(this.deltaHorizontal);
        EditorGUILayout.PropertyField(this.restartScene);

        if (((GameObject)this.prefabModule.objectReferenceValue) != null && ((GameObject)this.prefabModule.objectReferenceValue).GetComponent<LoadModule>() == null)
        {
            ((GameObject)this.prefabModule.objectReferenceValue).AddComponent<LoadModule>();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
