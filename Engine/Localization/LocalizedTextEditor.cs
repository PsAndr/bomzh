#if UNITY_EDITOR
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

namespace Engine
{
    [CustomEditor(typeof(LocalizedText))]
    public class LocalizedTextEditor : Editor
    {
        private static class Constants
        {
            public const float heightTextArea = 75f;
        }

        SerializedProperty nameLocalizedText;
        SerializedProperty text;
        SerializedProperty typeGetingTexts;
        SerializedProperty helpToFileLoad;

        private bool isInit = false;

        private void OnDisable()
        {
            if (!Application.isPlaying) 
            { 
                LocalizedTextsControl localizedTextsControl = FindObjectOfType<LocalizedTextsControl>();
                if (localizedTextsControl == null)
                {
                    GameObject helperGameObject = new GameObject("___helperLocalizedTextsControl___", typeof(LocalizedTextsControl));
                    helperGameObject.GetComponent<LocalizedTextsControl>().Init();
                }
                else
                {
                    if (isInit)
                    {
                        localizedTextsControl.UpdateFileSave();
                        isInit = false;
                    }
                }
            }
        }

        private void OnEnable()
        {
            if (!Application.isPlaying)
            {
                isInit = true;
                nameLocalizedText = serializedObject.FindProperty("nameLocalizedText");
                typeGetingTexts = serializedObject.FindProperty("typeGetingTexts");
                helpToFileLoad = serializedObject.FindProperty("helpToFileLoad");

                LocalizedTextsControl localizedTextsControl = FindObjectOfType<LocalizedTextsControl>();
                if (localizedTextsControl == null)
                {
                    GameObject helperGameObject = new GameObject("___helperLocalizedTextsControl___", typeof(LocalizedTextsControl));
                    helperGameObject.GetComponent<LocalizedTextsControl>().Init();
                }
                else
                {
                    localizedTextsControl.FindTexts();
                    localizedTextsControl.CheckFileSave();
                    localizedTextsControl.UpdatePathsFiles();
                }
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            LocalizedText localizedText = (LocalizedText)target;

            //localizedText.ChangeName(EditorGUILayout.TextField("Name:", localizedText.GetName()));

            base.OnInspectorGUI();

            localizedText.UpdateTypeText();
            LocalizedTextsControl localizedTextsControl = FindObjectOfType<LocalizedTextsControl>();

            Dictionary<string, string> localizations = new Dictionary<string, string>(localizedText.GetLocalizationsTexts());

            foreach (KeyValuePair<string, string> kvp in localizations)
            {

                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField($"Localization text of {kvp.Key}:");
                localizedText.ChangeTypeGetingText(kvp.Key, (LocalizedText.TypeGetingText)EditorGUILayout.EnumPopup(localizedText.GetTypeGetingTexts()[kvp.Key]));

                if (localizedText.GetTypeGetingTexts()[kvp.Key] == LocalizedText.TypeGetingText.TextArea)
                {
                    localizedText.ChangeLocalizationText(kvp.Key, EditorGUILayout.TextArea(kvp.Value,
                        GUILayout.Height(Constants.heightTextArea), GUILayout.ExpandWidth(true)));
                    EditorGUILayout.EndVertical();
                }
                else
                {
                    bool flag = false;
                    if (GUILayout.Button("Choose file"))
                    {
                        flag = true;
                        string path = EditorUtility.OpenFilePanel("Choose file txt", Application.dataPath, "txt");
                        
                        if (File.Exists(path))
                        {
                            localizedText.ChangePathFile(kvp.Key, path);
                        }
                        else
                        {
                            localizedText.ChangePathFile(kvp.Key, null);
                        }

                        localizedTextsControl.UpdatePathsFiles();
                    }
                    if (!flag)
                    {
                        EditorGUILayout.Space(6f);

                        if (localizedText.GetFiles()[kvp.Key] == null)
                        {
                            EditorGUILayout.LabelField($"Path file now: None");
                        }
                        else
                        {
                            EditorGUILayout.LabelField($"Path file now: {localizedText.GetFiles()[kvp.Key]}");
                        }

                        EditorGUILayout.Space(6f);
                        if (GUILayout.Button("Update text in file"))
                        {
                            localizedText.UpdateTextInFile(kvp.Key);
                        }
                        if (GUILayout.Button("Update text from file"))
                        {
                            localizedText.UpdateTextFromFile(kvp.Key);
                        }
                        EditorGUILayout.EndVertical();
                    }
                }
            }

            EditorGUILayout.Space(25f);

            if (GUILayout.Button("Update File Save"))
            {
                FindObjectOfType<LocalizedTextsControl>().UpdateFileSave();
            }

            if (GUILayout.Button("Check File Save"))
            {
                FindObjectOfType<LocalizedTextsControl>().CheckFileSave();
                serializedObject.ApplyModifiedProperties();  
                serializedObject.Update();
            }

            EditorGUILayout.HelpBox($"Type text: {localizedText.GetTypeLocalizedText()}", MessageType.None);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
