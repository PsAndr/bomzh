using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using WorkWithDictionary;

[System.Serializable]
public class Save_class
{
    public string name_save;

    public int scene_number;
    public int number_command_scene;
    public DictionaryToTwoArrays<string, int> flags;

    public Save_class(string name_save, int scene_number, int number_command_scene, DictionaryToTwoArrays<string, int> flags)
    {
        this.name_save = name_save;
        this.scene_number = scene_number;
        this.number_command_scene = number_command_scene;
        this.flags = flags;

        this.WorkAfterInit();
    }

    public Save_class(string name_save, int scene_number, int number_command_scene, Dictionary<string, int> flags)
    {
        this.name_save = name_save;
        this.scene_number = scene_number;
        this.number_command_scene = number_command_scene;
        this.flags = new DictionaryToTwoArrays<string, int>(flags);

        this.WorkAfterInit();
    }

    public Save_class(int scene_number, int number_command_scene, Dictionary<string, int> flags)
    {
        this.name_save = DateTime.Now.ToString();
        this.scene_number = scene_number;
        this.number_command_scene = number_command_scene;
        this.flags = new DictionaryToTwoArrays<string, int>(flags);

        this.WorkAfterInit();
    }

    public Save_class(int scene_number, int number_command_scene, DictionaryToTwoArrays<string, int> flags)
    {
        this.name_save = DateTime.Now.ToString();
        this.scene_number = scene_number;
        this.number_command_scene = number_command_scene;
        this.flags = flags;

        this.WorkAfterInit();
    }

    public Save_class(string name_save)
    {
        this.name_save = name_save;
        load();
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
        string path = Application.persistentDataPath + "/Saves/" + this.name_save + ".save";

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(path, FileMode.Create);
        bf.Serialize(fs, this);
        fs.Close();

        Save_list_names save_List_Names = new Save_list_names();
        save_List_Names.AddName(this.name_save);
    }

    private void load()
    {
        string path = Application.persistentDataPath + "/Saves/" + this.name_save + ".save";

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(path, FileMode.Open);

        Save_class save_Class = (Save_class)bf.Deserialize(fs);
        fs.Close();

        this.flags = save_Class.flags;
        this.number_command_scene = save_Class.number_command_scene;
        this.scene_number = save_Class.scene_number;
    }

    private void delete()
    {
        File.Delete(Application.persistentDataPath + "/Saves/" + this.name_save + ".save");
    }

    public void Change(int scene_number, int number_command_scene, Dictionary<string, int> flags)
    {
        this.scene_number = scene_number;
        this.number_command_scene = number_command_scene;
        this.flags = new DictionaryToTwoArrays<string, int>(flags);

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
}

public class Save_list_names
{
    public string[] list_names;

    private string path = Application.persistentDataPath + "/list_names_saves.save";

    public Save_list_names(bool firstInit = false)
    {
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

