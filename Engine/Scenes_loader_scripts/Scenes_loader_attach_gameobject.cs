using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;

public class Scenes_loader_attach_gameobject : MonoBehaviour
{
    public TextMeshProUGUI obj;
    private void Awake()
    {
        Scenes_loader scenes_Finder = new Scenes_loader();
        obj.text = scenes_Finder.Scenes_dict[0].second;
    }
}
