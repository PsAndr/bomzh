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

    public void WorkAfterInit()
    {
        if (string.IsNullOrEmpty(this.name_save))
        {
            this.name_save = DateTime.Now.ToString();
        }

        string path = Application.persistentDataPath + "/list_names_saves.save";
        string[] list_names;

        if (!File.Exists(path))
        {
            list_names = new string[0];
        }
        else
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);

            list_names = (string[])bf.Deserialize(fs);

            fs.Close();
        }

        if (IsStringInArray(list_names, this.name_save))
        {
            int index = 0;
            while (IsStringInArray(list_names, this.name_save + "(" + index.ToString() + ")"))
            {
                index++;
            }

            this.name_save = this.name_save + "(" + index.ToString() + ")";
        }

        this.save();
    }

    public void save()
    {
        string path = Application.persistentDataPath + "/" + this.name_save + ".save";

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(path, FileMode.Create);
        bf.Serialize(fs, this);
        fs.Close();

        path = Application.persistentDataPath + "/list_names_saves.save";

        string[] list_names;

        if (!File.Exists(path))
        {
            list_names = new string[0];
        }
        else
        {
            bf = new BinaryFormatter();
            fs = new FileStream(path, FileMode.Open);

            list_names = (string[])bf.Deserialize(fs);

            fs.Close();
        }

        if (!IsStringInArray(list_names, this.name_save))
        {
            string[] list_names_new = new string[list_names.Length + 1];
            for (int i = 0; i < list_names.Length; i++)
            {
                list_names_new[i] = list_names[i];
            }
            list_names_new[list_names.Length] = this.name_save;
            list_names = (string[])list_names_new.Clone();    
        }

        bf = new BinaryFormatter();
        fs = new FileStream(path, FileMode.Create);
        bf.Serialize(fs, list_names);
        fs.Close();
    }

    private static bool IsStringInArray(string[] array, string str)
    {
        foreach (string str2 in array)
        {
            if (str == str2)
            {
                return true;
            }
        }

        return false;
    }
}


