using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;

public class Scenes_loader_attach_gameobject : MonoBehaviour
{
    public TextMeshProUGUI obj;
    [SerializeField] public Scene_class scene_Classes;
    private void Awake()
    {
        Scenes_loader scenes_Loader = new Scenes_loader();
    }
}
