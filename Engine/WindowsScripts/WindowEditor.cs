#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;
using UnityEditor;

[CustomEditor(typeof(Window))]
public class WindowEditor : Editor
{
    SerializedProperty animationClip_open;
    SerializedProperty animationClip_close;
    SerializedProperty setDefaultOpen;

    private void OnEnable()
    {
        this.animationClip_open = serializedObject.FindProperty("animationClip_open");
        this.animationClip_close = serializedObject.FindProperty("animationClip_close");
        this.setDefaultOpen = serializedObject.FindProperty("setDefaultOpen");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(animationClip_open);
        EditorGUILayout.PropertyField(animationClip_close);
        EditorGUILayout.PropertyField(setDefaultOpen);

        Window window = (Window)target;

        serializedObject.ApplyModifiedProperties();
        if (!Application.isPlaying)
        {
            window.UpdateDefaultOpenValues();
        }
    }
}
#endif