#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Engine
{
    [CustomEditor(typeof(SpeedTextPrintingChanger))]
    public class SpeedTextPrintingChangerEditor : Editor
    {
        SpeedTextPrintingChanger speedTextPrintingChanger;
        private void OnEnable()
        {
            speedTextPrintingChanger = (SpeedTextPrintingChanger)target;
            speedTextPrintingChanger.Init();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();
            speedTextPrintingChanger.UpdateValueSlider();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif