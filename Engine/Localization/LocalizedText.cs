using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

namespace Engine
{
    [AddComponentMenu("Engine/Localization/Localized Text")]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private string nameLocalizedText;
        [SerializeField] private GameObject text;

        [SerializeField, HideInInspector] private MyDictionary<string, TypeGetingText> typeGetingTexts;
        public enum TypeGetingText { TextArea, File };

        private Dictionary<string, string> localizations = new Dictionary<string, string>();

        [SerializeField, HideInInspector] private TypeLocalizedText m_TypeLocalizedText;
        [SerializeField, HideInInspector] private MyDictionary<string, string> fileLoadTexts = new MyDictionary<string, string>();

        public LocalizedText()
        {
        }

        public void UpdateLocalizationsNames(params string[] localizationNames)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            MyDictionary<string, TypeGetingText> result2 = new MyDictionary<string, TypeGetingText>();
            MyDictionary<string, string> result3 = new MyDictionary<string, string>();

            foreach (string name in localizationNames)
            {
                string toAdd = "";
                TypeGetingText typeGetingText = typeGetingTexts[name];
                string pathFile = fileLoadTexts[name];
#if UNITY_EDITOR
                if (!File.Exists(pathFile))
                {
                    pathFile = null;
                }
#endif

                if (this.localizations.ContainsKey(name))
                {
                    toAdd = this.localizations[name];
                }

                result.Add(name, toAdd);
                result2.Add(name, typeGetingText);
                result3.Add(name, pathFile);
            }

            this.localizations = result;
            this.typeGetingTexts = result2;
            this.fileLoadTexts = result3;
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
        }

        public void UpdateTextLocalization()
        {
            UpdateTypeText();

            string toText = this.localizations[FindObjectOfType<Global_control>().localization.GetLocalization()];

            switch (this.m_TypeLocalizedText)
            {
                case (TypeLocalizedText.Text):
                    this.text.GetComponent<Text>().text = toText;
                    break;

                case (TypeLocalizedText.TextMeshProUGUI):
                    this.text.GetComponent<TextMeshProUGUI>().text = toText;
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

        public MyDictionary<string, TypeGetingText> GetTypeGetingTexts()
        {
            return this.typeGetingTexts;
        }

        public void ChangeTypeGetingText(string name, TypeGetingText typeGetingText)
        {
            if (!this.typeGetingTexts.ContainsKey(name))
            {
                return;
            }
            this.typeGetingTexts[name] = typeGetingText;
        }

        public void ChangePathFile(string name, string path)
        {
            if (!this.fileLoadTexts.ContainsKey(name))
            {
                return;
            }
            this.fileLoadTexts[name] = path;
        }

        public void UpdateTextFromFile(string name)
        {
#if UNITY_EDITOR
            if (this.fileLoadTexts.ContainsKey(name) && File.Exists(this.fileLoadTexts[name]))
            {
                this.localizations[name] = File.ReadAllText(this.fileLoadTexts[name]);
                LocalizedTextsControl localizedTextsControl = FindObjectOfType<LocalizedTextsControl>();
                localizedTextsControl.UpdateFileSave();
            }
#endif
        }

        public void UpdateTextInFile(string name)
        {
#if UNITY_EDITOR
            if (this.fileLoadTexts.ContainsKey(name) && File.Exists(this.fileLoadTexts[name]))
            {
                LocalizedTextsControl localizedTextsControl = FindObjectOfType<LocalizedTextsControl>();
                localizedTextsControl.CheckFileSave();
                File.WriteAllText(this.fileLoadTexts[name], this.localizations[name]);
            }
#endif
        }

        public MyDictionary<string, string> GetFiles()
        {
            return this.fileLoadTexts;
        }
    }
}
