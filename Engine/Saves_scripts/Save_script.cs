using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using Engine.WorkWithDictionary;
using Engine.WorkWithRectTransform;
using UnityEngine.UI;
using Engine.Files;

namespace Engine
{
    [System.Serializable]
    public class Save_class
    {
        private static class Constants
        {
            public static readonly string pathSave = Application.persistentDataPath + "/Saves/";
            public static readonly string pathAutoSave = Application.persistentDataPath + "/AutoSaves/";
        }

        public string name_save;

        public int scene_number;
        public int number_command_scene;
        public MyDictionary<string, int> flags;
        public AudioHelper.SaveClass[] audioHelpers;
        public VideoHelper.SaveClass[] videoHelpers;

        public string nameBackground;

        public int indexPrint;
        public string textOnSceneDialogue;
        public string textOnSceneCharacter;

        public string[] spritesNames;
        public string[] spritesObjectNames;
        public RectTransformSaveValuesSerializable[] rectTransformsSprites;

        public Save_class() 
        { 
        
        }

        private void PasteValuesFromGlobalControl(Global_control global_Control)
        {
            AudioHelper[] audioHelpers = global_Control.audioHandler.gameObject.GetComponents<AudioHelper>();
            AudioHelper.SaveClass[] saveClasses = new AudioHelper.SaveClass[audioHelpers.Length];

            for (int i = 0; i < audioHelpers.Length; i++)
            {
                saveClasses[i] = audioHelpers[i].GetSave();
            }

            VideoHelper[] videoHelpers = global_Control.videoHandler.gameObject.GetComponents<VideoHelper>();
            VideoHelper.SaveClass[] saveClassesVideo = new VideoHelper.SaveClass[videoHelpers.Length];

            for (int i = 0; i < videoHelpers.Length; i++)
            {
                saveClassesVideo[i] = videoHelpers[i].GetSave();
            }

            Array.Sort(saveClassesVideo);

            Image[] sprites = global_Control.ToSpawnSprite.GetComponentsInChildren<Image>();
            List<string> spritesNames = new();
            List<string> spritesNamesObjects = new();
            List<RectTransformSaveValuesSerializable> rectTransformsSprites = new();

            for (int i = 0; i < sprites.Length; i++)
            {
                if (sprites[i].gameObject.name.Split("___").Length > 1 && sprites[i].gameObject.name.Split("___")[1] == "sprite")
                {
                    spritesNames.Add(sprites[i].sprite.name);
                    spritesNamesObjects.Add(sprites[i].gameObject.name);
                    rectTransformsSprites.Add(new RectTransformSaveValuesSerializable(sprites[i].gameObject.GetComponent<RectTransform>()));
                }
            }

            TextPrintingClass textPrintingClass = global_Control.gameObject.GetComponent<TextPrintingClass>();

            this.scene_number = global_Control.GetSceneValues().first;
            this.number_command_scene = global_Control.GetSceneValues().second;
            this.flags = global_Control.Flags;
            this.audioHelpers = saveClasses;
            this.nameBackground = global_Control.background.sprite.name;
            this.indexPrint = textPrintingClass.GetProgress();
            this.textOnSceneDialogue = global_Control.text_dialogue.text;
            this.textOnSceneCharacter = global_Control.text_character.text;
            this.spritesNames = spritesNames.ToArray();
            this.spritesObjectNames = spritesNamesObjects.ToArray();
            this.rectTransformsSprites = rectTransformsSprites.ToArray();
            this.videoHelpers = saveClassesVideo;
        }

        public Save_class(Global_control global_Control)
        {
            this.PasteValuesFromGlobalControl(global_Control);

            this.WorkAfterInit();
        }

        public static Save_class Load(string name_save)
        {
            Save_class toReturn = new Save_class();
            toReturn.name_save = name_save;
            toReturn.load();
            return toReturn;
        }

        private void WorkAfterInit()
        {
            if (string.IsNullOrEmpty(this.name_save))
            {
                this.name_save = DateTime.Now.ToString();
            }

            this.name_save = this.name_save.Replace(':', '-');

            Save_list_names save_List_Names = new Save_list_names();

            if (save_List_Names.ContainsName(this.name_save))
            {
                int index = 0;
                while (save_List_Names.ContainsName(this.name_save + "(" + index.ToString() + ")"))
                {
                    index++;
                }

                this.name_save = this.name_save + "(" + index.ToString() + ")";
            }

            this.save();
        }

        private void save()
        {
            WorkWithFiles.CheckDirectory(Constants.pathSave);
            string path = Constants.pathSave + this.name_save + ".save";

            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Create);
            bf.Serialize(fs, this);
            fs.Close();

            Save_list_names save_List_Names = new Save_list_names();
            save_List_Names.AddName(this.name_save);
        }

        private void load()
        {
            WorkWithFiles.CheckDirectory(Constants.pathSave);
            string path = Constants.pathSave + this.name_save + ".save";

            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);

            Save_class save_Class = (Save_class)bf.Deserialize(fs);
            fs.Close();

            this.flags = save_Class.flags;
            this.number_command_scene = save_Class.number_command_scene;
            this.scene_number = save_Class.scene_number;
            this.audioHelpers = save_Class.audioHelpers;
            this.nameBackground = save_Class.nameBackground;
            this.indexPrint = save_Class.indexPrint;
            this.textOnSceneCharacter = save_Class.textOnSceneCharacter;
            this.textOnSceneDialogue = save_Class.textOnSceneDialogue;
            this.spritesNames = save_Class.spritesNames;
            this.spritesObjectNames = save_Class.spritesObjectNames;
            this.rectTransformsSprites = save_Class.rectTransformsSprites;
            this.videoHelpers = save_Class.videoHelpers;
        }

        private void delete()
        {
            File.Delete(Constants.pathSave + this.name_save + ".save");
        }

        private void AutoSave()
        {
            WorkWithFiles.CheckDirectory(Constants.pathAutoSave);
            string path = Constants.pathAutoSave + this.name_save + ".save";

            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Create);
            bf.Serialize(fs, this);
            fs.Close();
        }

        public static void MakeAutoSave(Global_control global_Control, string nameAutoSave = "AutoSave")
        {
            Save_class autoSave = new();

            autoSave.PasteValuesFromGlobalControl(global_Control);
            autoSave.name_save = nameAutoSave;

            autoSave.AutoSave();
        }

        public void Change(Global_control global_Control)
        {
            this.PasteValuesFromGlobalControl(global_Control);

            this.save();
        }

        public void Change(string save_name)
        {
            delete();

            new Save_list_names().RemoveName(this.name_save);

            this.name_save = save_name;

            this.WorkAfterInit();
        }

        public void Delete()
        {
            delete();
            new Save_list_names().RemoveName(this.name_save);
        }

        public void Save()
        {
            WorkAfterInit();
        }
    }

    public class Save_list_names
    {
        public string[] list_names;

        private static readonly string path = Application.persistentDataPath + "/list_names_saves.save";

        public Save_list_names(bool firstInit = false)
        {
            WorkWithFiles.CheckDirectory(path);
            WorkWithFiles.CheckDirectory(Application.persistentDataPath + "/Saves");

            if (!File.Exists(path))
            {
                list_names = new string[0];

                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(path, FileMode.Create);

                bf.Serialize(fs, list_names);

                fs.Close();

                Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
            }
            else
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(path, FileMode.Open);

                list_names = (string[])bf.Deserialize(fs);

                fs.Close();
            }

            if (firstInit)
            {
                this.CheckNewFiles();
            }
        }

        private void CheckNewFiles()
        {
            string[] save_files = Directory.GetFiles(Application.persistentDataPath + "/Saves/", "*.save");

            foreach (string file in save_files)
            {
                string file_name = file.Split('/')[^1];
                file_name = file_name.Remove(file_name.IndexOf(".save"));

                if (!this.ContainsName(file_name))
                {
                    this.AddName(file_name);
                }
            }

            foreach (string name in this.list_names)
            {
                if (!File.Exists(Application.persistentDataPath + $"/Saves/{name}.save"))
                {
                    this.RemoveName(name);
                }
            }

            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Create);

            bf.Serialize(fs, list_names);

            fs.Close();
        }

        public bool ContainsName(string name)
        {
            foreach (string str in list_names)
            {
                if (str.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }

        public void AddName(string name)
        {
            List<string> list = new List<string>(list_names);

            if (!ContainsName(name))
            {
                list.Add(name);
            }

            this.list_names = list.ToArray();
            list = null;

            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Create);

            bf.Serialize(fs, list_names);

            fs.Close();
        }

        public void RemoveName(string name)
        {
            List<string> list = new List<string>(list_names);

            list.Remove(name);

            this.list_names = list.ToArray();

            list = null;

            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Create);

            bf.Serialize(fs, list_names);

            fs.Close();
        }
    }

    public class ScreenshotSaverLoader
    {
        private Dictionary<string, Sprite> screenshots;
        private string path = Application.persistentDataPath + "/Saves/";

        public ScreenshotSaverLoader()
        {
            WorkWithFiles.CheckDirectory(path);

            screenshots = new Dictionary<string, Sprite>();
            Save_list_names save_List_Names = new Save_list_names();
            foreach (string name in save_List_Names.list_names)
            {
                if (!File.Exists(path + $"{name}.jpg"))
                {
                    continue;
                }
                Texture2D texture2D = new Texture2D(1920, 1080, TextureFormat.RGB24, false);
                texture2D.LoadImage(File.ReadAllBytes(path + $"{name}.jpg"), false);
                Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
                this.screenshots.Add(name, sprite);
            }
        }

        public Sprite GetScreeenshot(string name)
        {
            if (!this.screenshots.ContainsKey(name))
            {
                return null;
            }
            return this.screenshots[name];
        }

        public void MakeScreenshot(string name, Global_control global_Control)
        {
            global_Control.MakeScreenshot(path + $"{name}.jpg", name);
        }

        public void Update(string name)
        {
            if (!File.Exists(path + $"{name}.jpg"))
            {
                return;
            }
            Texture2D texture2D = new Texture2D(1920, 1080, TextureFormat.RGB24, false);
            texture2D.LoadImage(File.ReadAllBytes(path + $"{name}.jpg"), false);
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));

            this.screenshots[name] = sprite;
        }

        public void Delete(string name)
        {
            if (this.screenshots.ContainsKey(name))
            {
                this.screenshots.Remove(name);
            }
            if (File.Exists(path + $"{name}.jpg"))
            {
                File.Delete(path + $"{name}.jpg");
            }
        }

        public void ChangeName(string namePrevious, string nameEnd)
        {
            if (File.Exists(path + $"{namePrevious}.jpg"))
            {
                File.Copy(path + $"{namePrevious}.jpg", path + $"{nameEnd}.jpg");
            }
            if (this.screenshots.ContainsKey(namePrevious))
            {
                if (!this.screenshots.ContainsKey(nameEnd))
                {
                    this.screenshots.Add(nameEnd, this.screenshots[namePrevious]);
                }
                else
                {
                    this.screenshots[nameEnd] = this.screenshots[namePrevious];
                }
                this.Delete(namePrevious);
            }
        }
    }
}
