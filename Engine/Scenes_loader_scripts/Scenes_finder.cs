using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Scenes_finder
{
    public string[] list_dirs;
    public Scenes_finder()
    {
        this.Init();
    }

    private void Init()
    {
        string path_myself = Application.dataPath;
        string path_my_parent = Directory.GetParent(path_myself).ToString();
        string path = path_myself + "\\Engine\\Scenes\\";

        this.list_dirs = Directory.GetDirectories(path, "Scene *");
    }
}
