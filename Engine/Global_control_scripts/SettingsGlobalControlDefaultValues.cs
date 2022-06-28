using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Engine
{
    [AddComponentMenu("Engine/Global Control/Settings Default Values")]
    public class SettingsGlobalControlDefaultValues : MonoBehaviour
    {
        [SerializeField] private Global_control global_Control;

        [SerializeField] private float speedTextPrinting;
        [SerializeField] private float minSpeedPrintingText;
        [SerializeField] private float maxSpeedPrintingText;
        public void UpdateValues()
        {
            if (global_Control == null)
            {
                global_Control = FindObjectOfType<Global_control>();
            }

            if (global_Control.settings == null)
            {
                global_Control.settings = new SettingsGlobalControl();
            }

            global_Control.settings.MinSpeedPrintingText = minSpeedPrintingText;
            global_Control.settings.MaxSpeedPrintingText = maxSpeedPrintingText;
            global_Control.settings.SpeedTextPrinting = speedTextPrinting;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SettingsGlobalControlDefaultValues))]
    public class SettingsGlobalControlDefaultValuesEditor : Editor
    {
        SettingsGlobalControlDefaultValues settings;

        private void Awake()
        {
            settings = (SettingsGlobalControlDefaultValues)target;
            if (!Application.isPlaying)
            {
                settings.UpdateValues();
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (!Application.isPlaying)
            {
                settings.UpdateValues();
            }
        }
    }
#endif
}

