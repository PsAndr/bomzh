using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace Engine
{
    public class SpeedTextPrintingChanger : MonoBehaviour
    {
        [SerializeField] private Global_control global_Control;

        [SerializeField] private Slider slider;

        private bool isInit = false;

        private float speedPrinting
        {
            get
            {
                return global_Control.settings.SpeedTextPrinting;
            }
            set
            {
                global_Control.settings.SpeedTextPrinting = value;
            }
        }

        public SpeedTextPrintingChanger()
        {
        }

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
                slider.onValueChanged.AddListener(ChangeSpeed);
            }

            isInit = true;
        }

        public void UpdateValueSlider()
        {
            slider.onValueChanged.RemoveListener(ChangeSpeed);
            slider.value = speedPrinting;
            slider.maxValue = global_Control.settings.MaxSpeedPrintingText;
            slider.minValue = global_Control.settings.MinSpeedPrintingText;
            slider.onValueChanged.AddListener(ChangeSpeed);
        }

        public void ChangeSpeed(float newSpeed)
        {
            speedPrinting = newSpeed;
        }
    }
}
