using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;

public class Global_control : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text_dialogue;

    [SerializeField] private Sprite background;

    private Scenes_loader scenes_Loader;

    //для управления сценами
    [SerializeField] private int scene_number_start = 0;
    [SerializeField] private string scene_name_start = null;

    private int scene_number;
    private string scene_name;

    private int number_command_scene;
    //

    private void Awake()
    {
        this.scenes_Loader = new Scenes_loader();
    }

    void Start()
    {
        
    }

    void Update()
    {

    }

    private void ChangeScene(int number, string name)
    {

    }

    private void SceneCommands()
    {
        Scene_class.DialogueOrChoiceOrCommand command = null;
        new HandlerCommandScene().SetCommand(this, command);
    }
}
