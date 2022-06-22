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
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();

            LocalizedText localizedText = (LocalizedText)target;
            
            localizedText.UpdateTypeText();

            EditorGUILayout.HelpBox($"Type text: {localizedText.GetTypeLocalizedText()}", MessageType.None);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
