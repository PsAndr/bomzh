using UnityEngine;
using UnityEngine.UI;
using System;

namespace Engine 
{
    [AddComponentMenu("Engine/Settings/Volume Changer")]
    public class VolumeChanger : MonoBehaviour
    {
        [SerializeField] private Global_control global_Control;

        [SerializeField] private Slider slider;

        private bool isInit = false;

        public VolumeChanger() { }

        private void Awake()
        {
            Init();
        }

        private void OnEnable()
        {
            UpdateValueSlider();
        }

        public void Init()
        {
            if (global_Control == null)
            {
                global_Control = FindObjectOfType<Global_control>();
                if (global_Control == null)
                {
                    throw new Exception("Don`t can find object with component Global Control");
                }
            }

            if (slider == null)
            {
                slider = GetComponentInChildren<Slider>();
                if (slider == null)
                {
                    throw new Exception("Don`t can find object with component Slider");
                }
            }

            if (slider != null && !isInit)
            {
                slider.onValueChanged.AddListener(ChangeVolume);
            }

            isInit = true;
        }

        public void UpdateValueSlider()
        {
            slider.onValueChanged.RemoveListener(ChangeVolume);
            slider.value = (global_Control.settings.Volume / 100f) * (slider.maxValue - slider.minValue) + slider.minValue;
            slider.onValueChanged.AddListener(ChangeVolume);
        }

        public void ChangeVolume(float newVolume)
        {
            global_Control.settings.Volume = (newVolume - slider.minValue) / (slider.maxValue - slider.minValue) * 100f;
        }
    }
}
