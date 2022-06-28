using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Engine.WorkWithJSON;

namespace Engine
{
    public class LocalizedTextsControl : MonoBehaviour
    {
        private static class Constants
        {
            public static readonly string pathSave = Application.dataPath + "/Resources/LocalizeSave/Localize.txt";
            public static readonly string pathSavePathsFiles = Application.dataPath + "/Resources/LocalizeSave/LocalizePathsFiles.json";
            public const string pathSaveResources = "LocalizeSave/Localize";

            public static readonly System.Text.Encoding encoding = System.Text.Encoding.UTF8;
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

        public void UpdatePathsFiles()
        {
#if UNITY_EDITOR
            MyDictionary<string, MyDictionary<string, string>> pathsFiles = new();

            foreach (LocalizedText text in texts)
            {
                var item = text.GetFiles();
                if (item == null)
                {
                    item = new();
                }
                pathsFiles.Add(text.GetName(), (MyDictionary<string, string>)item.Clone());
            }

            if (!Directory.Exists(Constants.pathSavePathsFiles[..Constants.pathSavePathsFiles.LastIndexOf('/')]))
            {
                Directory.CreateDirectory(Constants.pathSavePathsFiles[..Constants.pathSavePathsFiles.LastIndexOf('/')]);
            }
            File.WriteAllText(Constants.pathSavePathsFiles, pathsFiles.ToJSON());
#endif
        }

        public void CheckPathsFiles()
        {
#if UNITY_EDITOR
            if (!Directory.Exists(Constants.pathSavePathsFiles[..Constants.pathSavePathsFiles.LastIndexOf('/')]))
            {
                Directory.CreateDirectory(Constants.pathSavePathsFiles[..Constants.pathSavePathsFiles.LastIndexOf('/')]);
            }

            if (!File.Exists(Constants.pathSavePathsFiles))
            {
                File.WriteAllText(Constants.pathSavePathsFiles, new MyDictionary<string, MyDictionary<string, string>>().ToJSON());
            }

            MyDictionary<string, MyDictionary<string, string>> paths = MyDictionary<string, MyDictionary<string, string>>.FromJSON(File.ReadAllText(Constants.pathSavePathsFiles));

            foreach (LocalizedText text in texts)
            {
                if (paths.ContainsKey(text.GetName()))
                {
                    foreach ((string name, string path) in paths[text.GetName()].GetValues())
                    {
                        text.ChangePathFile(name, path);
                    }
                }
            }
#endif
        }

        public void CheckFileSave()
        {
            #if UNITY_EDITOR
            if (!Directory.Exists(Constants.pathSave[..Constants.pathSave.LastIndexOf('/')]))
            {
                Directory.CreateDirectory(Constants.pathSave[..Constants.pathSave.LastIndexOf('/')]);
            }

            if (!File.Exists(Constants.pathSave))
            {
                UpdateFileSave();
                return;
            }
            #endif

            FindTexts();

#if UNITY_EDITOR
            Dictionary<string, Dictionary<string, string>> textFromSave = Localize.Convert.ConvertFrom(File.ReadAllText(Constants.pathSave, Constants.encoding));
#else
            Dictionary<string, Dictionary<string, string>> textFromSave = Localize.Convert.ConvertFrom(Resources.Load<TextAsset>(Constants.pathSaveResources).text);
#endif
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
#if UNITY_EDITOR
            File.WriteAllText(Constants.pathSave, Localize.Convert.ConvertTo(texts), Constants.encoding);
#endif
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
                Debug.Log(toReturn);
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
                Debug.Log(text);
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
