#if UNITY_EDITOR
using System.Collections;
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

        private void OnDisable()
        {
            LocalizedTextsControl localizedTextsControl = FindObjectOfType<LocalizedTextsControl>();
            if (localizedTextsControl == null)
            {
                GameObject helperGameObject = new GameObject("___helperLocalizedTextsControl___", typeof(LocalizedTextsControl));
                helperGameObject.GetComponent<LocalizedTextsControl>().Init();
            }
            else
            {
                localizedTextsControl.UpdateFileSave(); 
            }
        }

        private void OnEnable()
        {
            nameLocalizedText = serializedObject.FindProperty("nameLocalizedText");


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
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            LocalizedText localizedText = (LocalizedText)target;

            //localizedText.ChangeName(EditorGUILayout.TextField("Name:", localizedText.GetName()));

            base.OnInspectorGUI();

            localizedText.UpdateTypeText();

            Dictionary<string, string> localizations = new Dictionary<string, string>(localizedText.GetLocalizationsTexts());

            foreach (KeyValuePair<string, string> kvp in localizations)
            {

                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField($"Localization text of {kvp.Key}:");
                localizedText.ChangeLocalizationText(kvp.Key, EditorGUILayout.TextArea(kvp.Value, 
                    GUILayout.Height(Constants.heightTextArea), GUILayout.ExpandWidth(true)));

                EditorGUILayout.EndVertical();
            }

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
