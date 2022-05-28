using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;
using UnityEditor;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

public class Global_control : MonoBehaviour
{
    [SerializeField] public GameObject text_dialogue;
    [SerializeField] public TextMeshProUGUI text_character;

    [SerializeField] public GameObject canvas;

    [SerializeField] public GameObject button;
    [SerializeField] public GameObject button_field;

    [SerializeField] public GameObject ToSpawnSprite;

    [SerializeField] public Image background;

    [SerializeField] public GameObject prefab_sprites;

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

    public BackgroundsLoader backgroundsLoader;
    public SpritesLoader spritesLoader;

    [SerializeField] public float speed_printing_text = 6f;

    public HandlerCommandScene handlerCommandScene = new HandlerCommandScene();

    private void Awake()
    {
        this.backgroundsLoader = new BackgroundsLoader();
        this.scenes_Loader = new Scenes_loader();
        this.spritesLoader = new SpritesLoader();
        Flags = new Dictionary<string, int>();
    }

    void Start()
    {
        this.ChangeScene(this.scene_number_start, this.scene_name_start);
    }

    void Update()
    {

    }

    public void ChangeScene(int number, string name, int num_command = 0)
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

        this.number_command_scene = num_command;

        this.SceneCommands();
    }

    public void NewCommandScene()
    {
        
        if (this.handlerCommandScene.CanDoNextCommand())
        {
            this.number_command_scene++;
            this.SceneCommands();
        }
        else if (this.handlerCommandScene.IsPrintingText)
        {
            gameObject.GetComponent<TextPrintingClass>().StopPrinting();
        }
    }

    private void SceneCommands()
    {
        if (number_command_scene >= this.scenes_Loader.Scenes_dict[this.scene_number].parts_scene.Length)
        {
            number_command_scene = this.scenes_Loader.Scenes_dict[this.scene_number].parts_scene.Length;
            Debug.LogWarning("The end");
            return;
        }

        Scene_class.DialogueOrChoiceOrCommand command = this.scenes_Loader.Scenes_dict[this.scene_number].parts_scene[this.number_command_scene];

        handlerCommandScene.SetCommand(this, command);
    }

    public GameObject SpawnObject(GameObject prefab, Vector3 position, Vector3 size, Vector3 rotation, string name, Transform parent)
    {
        GameObject spawn_object = Instantiate(prefab, position, Quaternion.identity, parent);

        spawn_object.transform.localPosition = position;

        if (!string.IsNullOrEmpty(name))
        {
            spawn_object.name = name;
        }

        Quaternion quaternion = Quaternion.Euler(rotation);
        spawn_object.transform.localRotation = quaternion;

        spawn_object.transform.localScale = size;

        return spawn_object;
    }

    public void DestroyObject(GameObject gameObject, float time_to_destroy)
    {
        Destroy(gameObject, time_to_destroy);
    }

    public void DestroyObject(Transform whereObjectIs, string name)
    {
        Destroy(whereObjectIs.Find(name).gameObject);
    }

    public void DestroyAllObjects(Transform from)
    {
        for (int i = 0; i < from.childCount; i++)
        {
            Destroy(from.GetChild(i).gameObject);
        }
    }
}
