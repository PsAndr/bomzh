#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Engine
{
    [CustomEditor(typeof(Localization))]
    public class LocalizationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();

            if (((Localization)target).GetLocalization() != null)
            {
                EditorGUILayout.HelpBox($"Localization now: {((Localization)target).GetLocalization()}", MessageType.None);
            }
            else
            {
                EditorGUILayout.HelpBox($"Localization now: None", MessageType.None);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif