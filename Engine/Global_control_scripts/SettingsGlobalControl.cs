using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Engine
{
    [System.Serializable]
    public class SettingsGlobalControl : System.ICloneable
    {
        private static class Constants
        {
            public static readonly string pathSave = Application.persistentDataPath + "/settings.save";
        }
        //if add some properties, add it at Clone and PasteValues functions

        [SerializeField] private float speedTextPrinting;
        [SerializeField] private float maxSpeedPrintingText;
        [SerializeField] private float minSpeedPrintingText;

        [SerializeField] private TypesSkiping typeSkiping;

        public TypesSkiping TypeSkiping
        {
            get { return typeSkiping; }
            set { typeSkiping = value; UpdateSave(); }
        }

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

                UpdateSave();
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

                UpdateSave();
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

                UpdateSave();
            }
        }

        private void UpdateSave()
        {
            FileStream fs = new FileStream(Constants.pathSave, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(fs, this);

            fs.Close();
        }

        public void LoadFromSave()
        {
            if (File.Exists(Constants.pathSave))
            {
                FileStream fs = new FileStream(Constants.pathSave, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();

                SettingsGlobalControl values = (SettingsGlobalControl)bf.Deserialize(fs); 
                this.PasteValues(values);

                fs.Close();
            }
            else
            {
                UpdateSave();
            }
        }

        public void PasteValues(SettingsGlobalControl values)
        {
            this.typeSkiping = values.typeSkiping;
            this.maxSpeedPrintingText = values.maxSpeedPrintingText;
            this.minSpeedPrintingText = values.minSpeedPrintingText;
            this.speedTextPrinting = values.speedTextPrinting;
        }

        public object Clone()
        {
            return new SettingsGlobalControl 
            { 
                minSpeedPrintingText = minSpeedPrintingText, 
                maxSpeedPrintingText = maxSpeedPrintingText, 
                speedTextPrinting = speedTextPrinting,
                typeSkiping = typeSkiping,
            };
        }
    }
}
