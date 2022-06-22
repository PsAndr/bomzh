using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Engine
{
    [CreateAssetMenu(fileName = "Localization", menuName = "Engine/Localization", order = 0)]
    public class Localization : ScriptableObject
    {
        private static class Constants
        {
            public static readonly string pathSave = Application.persistentDataPath + "/localization.save";
        }

        [SerializeField] private string[] localizations = new string[0];

        private int index;

        public Localization(int index, params string[] localizations)
        {
            this.index = index;
            this.localizations = localizations;
        }

        public Localization() { }

        public void Change(string name)
        {
            int i = 0;
            foreach (string s in localizations)
            {
                if (s == name)
                {
                    this.index = i;
                    break;
                }
                i++;
            }

            UpdateSave();
        }

        public void ChangeLocalizations(params string[] localizations)
        {
            this.localizations = localizations;
            this.GetLocalization();

            UpdateSave();
        }

        public void Change(int index)
        {
            if (index >= 0 && index < this.localizations.Length)
            {
                this.index = index;
            }

            UpdateSave();
        }

        [System.Serializable]
        private class SaveClassLocalization
        {
            public string[] localizations = new string[0];
            public int index;

            public SaveClassLocalization(int index, params string[] localizations)
            {
                this.index = index;
                this.localizations = localizations;
            }

            public SaveClassLocalization(Localization localization)
            {
                this.index = localization.index;
                this.localizations = localization.localizations;
            }
        }

        public void GetValuesFromSave()
        {
            if (!File.Exists(Constants.pathSave))
            {
                UpdateSave();

                return;
            }
            else
            {
                FileStream fs = new FileStream(Constants.pathSave, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();

                SaveClassLocalization saveClass = (SaveClassLocalization)bf.Deserialize(fs);

                fs.Close();

                this.Change(saveClass.index);
                this.ChangeLocalizations(saveClass.localizations);
            }
        }

        private void UpdateSave()
        {
            FileStream fs = new FileStream(Constants.pathSave, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(fs, new SaveClassLocalization(this));

            fs.Close();
        }

        public string GetLocalization()
        {
            if (this.index < 0)
            {
                this.index = 0;
            }
            if (this.index >= this.localizations.Length)
            {
                index = this.localizations.Length - 1;
            }
            if (index < 0)
            {
                return null;
            }
            if (string.IsNullOrEmpty(this.localizations[index]))
            {
                return null;
            }
            return this.localizations[index];
        }

        public string[] GetLocalizations()
        {
            return this.localizations;
        }

        public int GetIndex()
        {
            return this.index;
        }
    }
}
