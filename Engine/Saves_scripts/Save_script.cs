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

    public string scene_name;
    public int number_command_scene;
    public DictionaryToTwoArrays<string, int> flags;

    public Save_class(string name_save, string scene_name, int number_command_scene, DictionaryToTwoArrays<string, int> flags)
    {
        this.name_save = name_save;
        this.scene_name = scene_name;
        this.number_command_scene = number_command_scene;
        this.flags = flags;

        this.WorkAfterInit();
    }

    public Save_class(string name_save, string scene_name, int number_command_scene, Dictionary<string, int> flags)
    {
        this.name_save = name_save;
        this.scene_name = scene_name;
        this.number_command_scene = number_command_scene;
        this.flags = new DictionaryToTwoArrays<string, int>(flags);

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
        this.scene_name = save_Class.scene_name;
    }

    private void delete()
    {
        File.Delete(Application.persistentDataPath + "/Saves/" + this.name_save + ".save");
    }

    public void Change(string scene_name, int number_command_scene, Dictionary<string, int> flags)
    {
        this.scene_name = scene_name;
        this.number_command_scene = number_command_scene;
        this.flags = new DictionaryToTwoArrays<string, int>(flags);

        this.save();
    }

    public void Change(string save_name)
    {
        delete();

        this.name_save = save_name;

        this.save();
    }
}

public class Save_list_names
{
    public string[] list_names;

    private string path = Application.persistentDataPath + "/list_names_saves.save";

    public Save_list_names()
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


