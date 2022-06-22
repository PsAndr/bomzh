using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    /// <summary>
    /// Helper class to make choose for choose window
    /// </summary>
    [System.Serializable]
    public class ChooseHelperClass
    {
        [SerializeField] private string[] selectionOptions;
        private int index;

        public ChooseHelperClass(int index, params string[] selectionOptions)
        {
            this.index = index; 
            this.selectionOptions = selectionOptions;
        }

        public static explicit operator ChooseHelperClass(Localization localization) 
        { 
            return new ChooseHelperClass(localization.GetIndex(), localization.GetLocalizations()); 
        }

        public static implicit operator Localization(ChooseHelperClass chooseHelperClass)
        {
            Localization localization = ScriptableObject.CreateInstance<Localization>();

            localization.Change(chooseHelperClass.index);
            localization.ChangeLocalizations(chooseHelperClass.selectionOptions);

            return localization;
        }

        public void PasteValues(ref Object obj)
        {
            if (obj.GetType() == typeof(Localization))
            {
                ((Localization)obj).Change(this.index);
                ((Localization)obj).ChangeLocalizations(this.selectionOptions);
            }
            else if (obj.GetType() == typeof(ChooseHelperClass))
            {
                ((ChooseHelperClass)obj).ChangeIndex(this.index);
                ((ChooseHelperClass)obj).ChangeOptions(this.selectionOptions);
            }
        }

        public static implicit operator string[](ChooseHelperClass chooseHelperClass)
        {
            return chooseHelperClass.selectionOptions;
        }

        public static implicit operator DynamicArray<string>(ChooseHelperClass chooseHelperClass)
        {
            return new DynamicArray<string>(chooseHelperClass.selectionOptions);
        }

        public string[] GetSelectionOptions()
        {
            return this.selectionOptions;
        }

        public int GetIndex()
        {
            return this.index;
        }

        public void ChangeIndex(int newIndex)
        {
            this.index = newIndex;
        }

        public void ChangeOptions(params string[] options)
        {
            this.selectionOptions = options;
        }
    }
}
