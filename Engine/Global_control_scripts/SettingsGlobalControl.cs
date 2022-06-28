using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    [System.Serializable]
    public class SettingsGlobalControl : System.ICloneable
    {
        [SerializeField, HideInInspector] private float speedTextPrinting;
        [SerializeField, HideInInspector] private float maxSpeedPrintingText;
        [SerializeField, HideInInspector] private float minSpeedPrintingText;

        public float SpeedTextPrinting
        {
            get { return speedTextPrinting; }
            set
            {
                if (value < minSpeedPrintingText)
                {
                    value = minSpeedPrintingText;
                }
                if (value > maxSpeedPrintingText)
                {
                    value = maxSpeedPrintingText;
                }
                speedTextPrinting = value;
            }
        }
        public float MaxSpeedPrintingText
        {
            get { return maxSpeedPrintingText; }
            set
            {
                if (value < minSpeedPrintingText)
                {
                    value = minSpeedPrintingText;
                }
                maxSpeedPrintingText = value;
            }
        }
        public float MinSpeedPrintingText
        {
            get { return minSpeedPrintingText; }
            set
            {
                if (value < 0.1f)
                {
                    value = 0.1f;
                }
                if (value > maxSpeedPrintingText)
                {
                    value = maxSpeedPrintingText;
                }
                minSpeedPrintingText = value;
            }
        }

        public object Clone()
        {
            return new SettingsGlobalControl 
            { 
                minSpeedPrintingText = minSpeedPrintingText, 
                maxSpeedPrintingText = maxSpeedPrintingText, 
                speedTextPrinting = speedTextPrinting 
            };
        }
    }
}
