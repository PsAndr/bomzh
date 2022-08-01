#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Engine
{
    [CustomEditor(typeof(ChooseOpenButton))]
    public class ChooseOpenButtonEditor : Editor
    {
        SerializedProperty valuesGetFrom;

        private void OnEnable()
        {
            valuesGetFrom = serializedObject.FindProperty("valuesGetFrom");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            ChooseOpenButton chooseOpenButton = (ChooseOpenButton)target;

            if (chooseOpenButton.typeChooseOpenButton == ChooseOpenButton.TypeChooseOpenButton.ValuesFromObject)
            {
                EditorGUILayout.Space(5);
                EditorGUILayout.PropertyField(valuesGetFrom);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif