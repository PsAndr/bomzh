using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    [CreateAssetMenu(fileName = "Localization", menuName = "Engine/Localization", order = 0)]
    public class Localization : ScriptableObject
    {
        [SerializeField] private string[] localizations = new string[0];

        private int index;

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
        }

        public void Change(int index)
        {
            if (index >= 0 && index < this.localizations.Length)
            {
                this.index = index;
            }
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
    }
}
