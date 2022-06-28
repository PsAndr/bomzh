using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Engine
{
    [AddComponentMenu("Engine/Get Engine Value/Get Engine Value")]
    public class GetEngineValue : MonoBehaviour
    {
        public enum ValueGetEnum
        {
            SpeedPrinting,
            MaxSpeedPrinting,
            MinSpeedPrinting,
            Flag,
        }

        public enum ConvertToStringEnum
        {
            No,
            Yes,
        }

        [SerializeField] private Global_control globalControl;
        [SerializeField] private ValueGetEnum valueGet;
        [SerializeField] private ConvertToStringEnum convertToString;

        public ValueGetEnum ValueGet { get { return valueGet; } }
        public ConvertToStringEnum ConvertToString { get { return convertToString; } }
        public Global_control GlobalControl { get { return globalControl; } set { globalControl = value != null ? value : globalControl; } }

        [SerializeField, HideInInspector] private string nameFlag;

        [SerializeField, HideInInspector] private GetEngineValueEvent<float> getEngineValue_float;
        [SerializeField, HideInInspector] private GetEngineValueEvent<int> getEngineValue_int;
        [SerializeField, HideInInspector] private GetEngineValueEvent<string> getEngineValue_string;

        [System.Serializable]
        private class GetEngineValueEvent<T> : UnityEvent<T> { }

        private void Awake()
        {
            UpdateValues();
        }

        private void Update()
        {
            UpdateValues();
        }

        private void UpdateValues()
        {
            switch (valueGet)
            {
                case ValueGetEnum.SpeedPrinting:
                    FloatValue(globalControl.settings.SpeedTextPrinting);
                    break;

                case ValueGetEnum.MinSpeedPrinting:
                    FloatValue(globalControl.settings.MinSpeedPrintingText);
                    break;

                case ValueGetEnum.MaxSpeedPrinting:
                    FloatValue(globalControl.settings.MaxSpeedPrintingText);
                    break;

                case ValueGetEnum.Flag:
                    if (globalControl.Flags.ContainsKey(nameFlag)) 
                    {
                        FloatValue(globalControl.Flags[nameFlag]);
                    }
                    else
                    {
                        FloatValue(0f);
                    }
                    break;
            }
        }

        private void IntValue(int value)
        {
            if (convertToString == ConvertToStringEnum.Yes)
            {
                StringValue(value.ToString());
                return;
            }
            getEngineValue_int.Invoke(value);
        }
        private void FloatValue(float value)
        {
            if (convertToString == ConvertToStringEnum.Yes)
            {
                StringValue(value.ToString());
                return;
            }
            getEngineValue_float.Invoke(value);
        }
        private void StringValue(string value)
        {
            getEngineValue_string.Invoke(value);
        }
    }
}
