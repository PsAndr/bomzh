#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Engine
{
    [CustomEditor(typeof(GetEngineValue))]
    public class GetEngineValueEditor : Editor
    {
        SerializedProperty getEngineValue_float;
        SerializedProperty getEngineValue_int;
        SerializedProperty getEngineValue_string;
        SerializedProperty nameFlag;
        GetEngineValue getEngineValue;

        private void OnEnable()
        {
            getEngineValue_float = serializedObject.FindProperty("getEngineValue_float");
            getEngineValue_int = serializedObject.FindProperty("getEngineValue_int");
            getEngineValue_string = serializedObject.FindProperty("getEngineValue_string");

            nameFlag = serializedObject.FindProperty("nameFlag");
            getEngineValue = (GetEngineValue)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();

            switch (getEngineValue.ValueGet)
            {
                case GetEngineValue.ValueGetEnum.SpeedPrinting:
                    FloatProperties();
                    break;

                case GetEngineValue.ValueGetEnum.MaxSpeedPrinting:
                    FloatProperties();
                    break;

                case GetEngineValue.ValueGetEnum.MinSpeedPrinting:
                    FloatProperties();
                    break;

                case GetEngineValue.ValueGetEnum.Flag:
                    FlagNameShow();
                    IntProperties();
                    break;
            }

            if (getEngineValue.GlobalControl == null)
            {
                getEngineValue.GlobalControl = FindObjectOfType<Global_control>();
            }

            serializedObject.ApplyModifiedProperties();
        }

        public void IntProperties()
        {
            if (getEngineValue.ConvertToString == GetEngineValue.ConvertToStringEnum.Yes)
            {
                StringProperties();
                return;
            }
            EditorGUILayout.PropertyField(getEngineValue_int);
        }
        public void StringProperties()
        {
            EditorGUILayout.PropertyField(getEngineValue_string);
        }
        public void FloatProperties()
        {
            if (getEngineValue.ConvertToString == GetEngineValue.ConvertToStringEnum.Yes)
            {
                StringProperties();
                return;
            }
            EditorGUILayout.PropertyField(getEngineValue_float);
        }
        public void FlagNameShow()
        {
            EditorGUILayout.PropertyField(nameFlag);
        }
    }
}
#endif