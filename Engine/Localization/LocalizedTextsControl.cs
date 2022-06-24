using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Engine
{
    public class LocalizedTextsControl : MonoBehaviour
    {
        private static class Constants
        {
            public static readonly string pathSave = Application.dataPath + "/Resources/Localize.txt";
            public static readonly string pathSaveResources = "Localize";
        }

        private LocalizedText[] texts;

        public void Init()
        {
            FindTexts();
            CheckFileSave();
        }

        public void FindTexts()
        {
            this.texts = FindObjectsOfType<LocalizedText>(true);

            foreach (LocalizedText text in texts)
            {
                text.UpdateLocalizationsNames(FindObjectOfType<Global_control>().localization.GetLocalizations());
            }
        }

        public void CheckFileSave()
        {
            if (Application.isEditor)
            {
                if (!File.Exists(Constants.pathSave))
                {
                    UpdateFileSave();
                    return;
                }
            }

            FindTexts();

            Dictionary<string, Dictionary<string, string>> textFromSave = Localize.Convert.ConvertFrom(Resources.Load<TextAsset>(Constants.pathSaveResources).text);

            string[] names = new string[this.texts.Length];

            for (int i = 0; i < names.Length; i++)
            {
                names[i] = this.texts[i].GetName();
            }

            foreach (KeyValuePair<string, Dictionary<string, string>> text in textFromSave)
            {
                Dictionary<string, string> textLocalizations = text.Value;

                int index = FindInArray.Find(text.Key, ref names);

                if (index != -1)
                {
                    this.texts[index].UpdateTexts(text.Value);
                }
            }


            foreach (LocalizedText localizedText in this.texts)
            {
                localizedText.UpdateTextLocalization();
            }
        }

        public void UpdateFileSave()
        {
            FindTexts();
            File.WriteAllText(Constants.pathSave, Localize.Convert.ConvertTo(texts));
        }
    }

    namespace Localize
    {
        public static class Convert
        {
            public static string ConvertTo(params LocalizedText[] localizedTexts)
            {
                string toReturn = string.Empty;

                foreach (LocalizedText text in localizedTexts)
                {
                    toReturn += $"{{{text.GetName()}\n";
                    foreach (KeyValuePair<string, string> kvp in text.GetLocalizationsTexts())
                    {
                        toReturn += $"[{kvp.Key}\n{kvp.Value}\n]\n";
                    }
                    toReturn += $"}}\n";
                }

                return toReturn;
            }

            private enum TypeSaveTextNow
            {
                None, 
                NewText,
                NewTextLocalization
            }

            public static Dictionary<string, Dictionary<string, string>> ConvertFrom(string text)
            {
                Dictionary<string, Dictionary<string, string>> toReturn = new Dictionary<string, Dictionary<string, string>>();

                string[] textSplitLines = text.Split('\n');

                TypeSaveTextNow typeSaveTextNow = TypeSaveTextNow.None;

                string textNow = "";
                string localization = "";
                string localizationText = "";

                foreach (string line in textSplitLines)
                {
                    switch (typeSaveTextNow)
                    {
                        case (TypeSaveTextNow.None):
                            
                            if (!string.IsNullOrEmpty(line) && line.Length > 0 && line[0] == '{')
                            {
                                textNow = line[1..];
                                typeSaveTextNow = TypeSaveTextNow.NewText;
                            }

                            break;

                        case (TypeSaveTextNow.NewText):
                            
                            if (!string.IsNullOrEmpty(line) && line.Length > 0 && line[0] == '[')
                            {
                                localization = line[1..];
                                typeSaveTextNow = TypeSaveTextNow.NewTextLocalization;
                            }
                            else if (!string.IsNullOrEmpty(line) && line.Length > 0 && line[0] == '}')
                            {
                                textNow = "";

                                typeSaveTextNow = TypeSaveTextNow.None;
                            }
                            
                            break;

                        case (TypeSaveTextNow.NewTextLocalization):

                            if (!string.IsNullOrEmpty(line) && line.Length > 0 && line[0] == ']')
                            {
                                if (localizationText.Length > 0)
                                {
                                    localizationText = localizationText[..^1];
                                }

                                if (toReturn.ContainsKey(textNow))
                                {
                                    toReturn[textNow][localization] = localizationText;
                                }
                                else
                                {
                                    Dictionary<string, string> map = new Dictionary<string, string>();
                                    map.Add(localization, localizationText);

                                    toReturn.Add(textNow, map);
                                }

                                localizationText = "";
                                localization = "";
                                typeSaveTextNow = TypeSaveTextNow.NewText;
                            }
                            else
                            {
                                localizationText += line + '\n';
                            }

                            break;
                    }
                }

                return toReturn;
            }
        }
    }
}
