#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Engine
{
    [CustomEditor(typeof(Global_control))]
    public class GlobalControlEditor : Editor
    {
        private void OnEnable()
        {
            
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();

            if (GUILayout.Button("Update start scene values"))
            {
                Global_control global_Control = (Global_control)target;
                global_Control.SaveStartSceneValues();
            }

            if (GUILayout.Button("Update files"))
            {
                Global_control global_Control = (Global_control)target;
                global_Control.UpdateFiles();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif