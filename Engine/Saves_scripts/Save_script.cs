using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

[System.Serializable]
public class Save_class
{
    public int num_scene;
    public bool[] array_flags;
    public string name_save;

    public Save_class(int num_scene, bool[] array_flags, string name_save)
    {
        this.num_scene = num_scene + 0;
        this.array_flags = (bool[])array_flags.Clone();
        this.name_save = name_save + ""; 
    }

    public Save_class(int num_scene, bool[] array_flags)
    {
        this.num_scene = num_scene + 0;
        this.array_flags = (bool[])array_flags.Clone();
        this.name_save = DateTime.Today.ToString(); //надо дописать выставление даты, как автомотическое имя
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

        bool flag = false;

        for (int i = 0; i < list_names.Length; i++)
        {
            string name = list_names[i];
            if (name.Equals(this.name_save))
            {
                flag = true;
            }
        }

        if (!flag)
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
}


