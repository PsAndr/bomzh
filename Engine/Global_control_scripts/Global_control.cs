using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;
using UnityEditor;

public class Global_control : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI text_dialogue;
    [SerializeField] public TextMeshProUGUI text_character;

    [SerializeField] public Sprite background;

    private Scenes_loader scenes_Loader;

    //для управления сценами
    [SerializeField] private int scene_number_start = -1;
    [SerializeField] private string scene_name_start = null;

    private int scene_number;
    private string scene_name;

    private int number_command_scene;

    public Dictionary<string, int> Flags;
    public List<string> Flags_name;
    //

    private void Awake()
    {
        this.scenes_Loader = new Scenes_loader();
        Flags = new Dictionary<string, int>();
    }

    void Start()
    {
        this.ChangeScene(this.scene_number_start, this.scene_name_start);
    }

    void Update()
    {

    }

    public void ChangeScene(int number, string name)
    {
        if (number == -1)
        {
            if (name == null)
            {
                foreach (KeyValuePair<int, Scene_class> kvp in this.scenes_Loader.Scenes_dict)
                {
                    number = kvp.Key;
                    break;
                }
            }
            else
            {
                number = this.scenes_Loader.Scenes_names_dict[name];
            }
        }

        this.scene_name = name;
        this.scene_number = number;

        this.number_command_scene = 0;

        this.SceneCommands();
    }

    public void NewCommandScene()
    {
        if (number_command_scene < this.scenes_Loader.Scenes_dict[this.scene_number].parts_scene.Length - 1)
        {
            this.number_command_scene++;
            this.SceneCommands();
        }
        else
        {
            Debug.LogWarning("The end");
        }
    }

    private void SceneCommands()
    {
        Scene_class.DialogueOrChoiceOrCommand command = this.scenes_Loader.Scenes_dict[this.scene_number].parts_scene[this.number_command_scene];
        new HandlerCommandScene().SetCommand(this, command);
    }
}
