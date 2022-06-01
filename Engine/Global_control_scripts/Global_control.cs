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

    [SerializeField] public Button ButtonScreen;

    [SerializeField] public GameObject button;
    [SerializeField] public GameObject button_field;

    [SerializeField] public GameObject ToSpawnSprite;

    [SerializeField] public Image background;

    [SerializeField] public GameObject prefab_sprites;

    [SerializeField] private GameObject canvasToScreenshot;
    [SerializeField] private Camera cameraToScreenshot;
    [SerializeField] private GameObject[] RenderToScreenshot;

    [SerializeField] public GameObject[] ObjectsStopLookScene;

    private Scenes_loader scenes_Loader;

    //для управления сценами
    [SerializeField] private int scene_number_start = -1;
    [SerializeField] private string scene_name_start = null;

    private int scene_number;
    private string scene_name;

    private int number_command_scene;

    [HideInInspector] public Dictionary<string, int> Flags;
    [HideInInspector] public List<string> Flags_name;
    //

    [HideInInspector] public BackgroundsLoader backgroundsLoader;
    [HideInInspector] public SpritesLoader spritesLoader;

    [SerializeField] public float speed_printing_text = 6f;

    [HideInInspector] public HandlerCommandScene handlerCommandScene;

    private void Awake()
    {
        this.backgroundsLoader = new BackgroundsLoader();
        this.scenes_Loader = new Scenes_loader();
        this.spritesLoader = new SpritesLoader();
        Flags = new Dictionary<string, int>();
        this.canvasToScreenshot.gameObject.SetActive(false);

        gameObject.AddComponent<TextPrintingClass>();

        handlerCommandScene = new HandlerCommandScene();
    }

    void Start()
    {
        this.ChangeScene(this.scene_number_start, this.scene_name_start);

        this.cameraToScreenshot.gameObject.SetActive(false);
    }

    void Update()
    {
        this.CheckObjectsStopLookScene();
    }

    private void CheckObjectsStopLookScene()
    {
        foreach (GameObject obj in this.ObjectsStopLookScene)
        {
            if (obj.activeSelf)
            {
                if (this.handlerCommandScene.IsLookScene)
                {
                    this.handlerCommandScene.StopLookScene(this);
                }
                return;
            }
        }
        if (!this.handlerCommandScene.IsLookScene)
        {
            this.handlerCommandScene.StartLookScene(this);
        }
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

    public void MakeScreenshot()
    {
        this.cameraToScreenshot.gameObject.SetActive(true);
        this.canvasToScreenshot.gameObject.SetActive(true);

        foreach (GameObject to_spawn in this.RenderToScreenshot)
        {
            this.SpawnObject(to_spawn, to_spawn.transform.localPosition, to_spawn.transform.localScale, to_spawn.transform.localEulerAngles, "ToScreenshot", this.canvasToScreenshot.transform);
        }

        StartCoroutine("WaitScreenshot");
    }

    IEnumerator WaitScreenshot()
    {
        yield return null;

        RenderTexture texture = this.cameraToScreenshot.targetTexture;

        Texture2D texture2D = new Texture2D(1920, 1080, TextureFormat.RGB24, false);

        RenderTexture.active = texture;

        texture2D.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
        texture2D.Apply();

        File.WriteAllBytes(Application.dataPath + "/screenshot.jpg", texture2D.EncodeToJPG());

        this.DestroyAllObjects(this.canvasToScreenshot.transform); 
        
        this.cameraToScreenshot.gameObject.SetActive(false);
        this.canvasToScreenshot.gameObject.SetActive(false);

        StopCoroutine("WaitScreenshot");
    }

    public Pair<string, int> GetSceneValues()
    {
        return new Pair<string, int>(this.scene_name, this.number_command_scene);
    }
}
