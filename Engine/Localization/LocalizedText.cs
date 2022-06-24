using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Engine
{
    [AddComponentMenu("Engine/Localization/Localized Text")]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private string nameLocalizedText;
        [SerializeField] private GameObject text;

        private Dictionary<string, string> localizations = new Dictionary<string, string>();

        private TypeLocalizedText m_TypeLocalizedText;

        public LocalizedText()
        {
        }

        public void UpdateLocalizationsNames(params string[] localizationNames)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (string name in localizationNames)
            {
                string toAdd = "";

                if (this.localizations.ContainsKey(name))
                {
                    toAdd = this.localizations[name];
                }

                result.Add(name, toAdd);
            }

            this.localizations = result;
        }

        public void UpdateTexts(Dictionary<string, string> namesAndTexts)
        {
            LocalizedTextsControl localizedTextsControl = FindObjectOfType<LocalizedTextsControl>();

            foreach (KeyValuePair<string, string> kvp in namesAndTexts)
            {
                if (this.localizations.ContainsKey(kvp.Key))
                {
                    this.localizations[kvp.Key] = kvp.Value;
                }
            }

            localizedTextsControl.UpdateFileSave();
        }

        public void UpdateTextLocalization()
        {
            UpdateTypeText();
            switch (this.m_TypeLocalizedText)
            {
                case (TypeLocalizedText.Text):
                    this.text.GetComponent<Text>().text = this.localizations[FindObjectOfType<Global_control>().localization.GetLocalization()];
                    break;

                case (TypeLocalizedText.TextMeshProUGUI):
                    this.text.GetComponent<TextMeshProUGUI>().text = this.localizations[FindObjectOfType<Global_control>().localization.GetLocalization()];
                    break;
            }
        }

        public enum TypeLocalizedText
        {
            None,
            Text,
            TextMeshProUGUI
        }

        public GameObject GetText()
        {
            return text;
        }

        public TypeLocalizedText GetTypeLocalizedText()
        {
            return m_TypeLocalizedText;
        }

        public void UpdateTypeText()
        {
            if (text == null)
            {
                m_TypeLocalizedText = TypeLocalizedText.None;
                if (gameObject.GetComponent<Text>() != null || gameObject.GetComponent<TextMeshProUGUI>() != null)
                {
                    text = gameObject;
                    UpdateTypeText();
                }
                return;
            }

            if (text.GetComponent<Text>() == null && text.GetComponent<TextMeshProUGUI>() == null)
            { 
                text = null;
            }
            else
            {
                if (text.GetComponent<TextMeshProUGUI>() != null)
                {
                    m_TypeLocalizedText = TypeLocalizedText.TextMeshProUGUI;
                }
                else if (text.GetComponent<Text>() != null)
                {
                    m_TypeLocalizedText = TypeLocalizedText.Text;
                }
            }
        }

        public string GetName()
        {
            return this.nameLocalizedText;
        }

        public Dictionary<string, string> GetLocalizationsTexts()
        {
            return this.localizations;
        }

        public void ChangeLocalizationText(string name, string newText)
        {
            if (!this.localizations.ContainsKey(name) || newText == null)
            {
                return;
            }
            this.localizations[name] = newText;
        }

        public void ChangeName(string newName)
        {
            this.nameLocalizedText = newName;
        }
    }
}
