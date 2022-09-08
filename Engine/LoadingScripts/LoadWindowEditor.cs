using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Linq;
using Engine;

#if UNITY_EDITOR
[CustomEditor(typeof(LoadWindow))]
public class LoadWindowEditor : Editor
{
    SerializedProperty startScene;
    SerializedProperty global_Control;
    SerializedProperty prefabModule;
    SerializedProperty countLines;
    SerializedProperty deltaHorizontal;
    SerializedProperty toSpawnModules;
    SerializedProperty sceneUnityController;

    private void OnEnable()
    {
        this.startScene = serializedObject.FindProperty("startScene");
        this.global_Control = serializedObject.FindProperty("global_Control");
        this.prefabModule = serializedObject.FindProperty("prefabModule");
        this.countLines = serializedObject.FindProperty("countLines");
        this.deltaHorizontal = serializedObject.FindProperty("deltaHorizontal");
        this.toSpawnModules = serializedObject.FindProperty("toSpawnModules");
        this.sceneUnityController = serializedObject.FindProperty("sceneUnityController");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        LoadWindow loadWindow = (LoadWindow)target;

        EditorGUILayout.PropertyField(this.global_Control);
        EditorGUILayout.PropertyField(this.sceneUnityController);
        EditorGUILayout.PropertyField(this.prefabModule);
        EditorGUILayout.PropertyField(this.toSpawnModules);
        EditorGUILayout.PropertyField(this.countLines);
        EditorGUILayout.PropertyField(this.deltaHorizontal);

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.PropertyField(this.startScene);

        if (this.startScene.boolValue)
        {
            GUIContent dropDown = new GUIContent("Choose scene");

            var scenesDirty = EditorBuildSettings.scenes;
            string[] scenes = new string[scenesDirty.Length];

            int i = 0;
            foreach (var sceneDirty in scenesDirty)
            {
                string name = sceneDirty.path.Split('/')[^1];
                name = name.Remove(name.IndexOf(".unity"));
                scenes[i] = name;
                i++;
            }

            loadWindow.sceneIndex = Mathf.Min(loadWindow.sceneIndex, scenes.Length - 1);
            loadWindow.sceneIndex = Mathf.Max(0, loadWindow.sceneIndex);

            loadWindow.sceneIndex = EditorGUILayout.Popup(dropDown, loadWindow.sceneIndex, scenes);

            loadWindow.newSceneStart = scenes[loadWindow.sceneIndex];
        }
        EditorGUILayout.EndVertical();

        if (((GameObject)this.prefabModule.objectReferenceValue) != null && ((GameObject)this.prefabModule.objectReferenceValue).GetComponent<LoadModule>() == null)
        {
            ((GameObject)this.prefabModule.objectReferenceValue).AddComponent<LoadModule>();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
