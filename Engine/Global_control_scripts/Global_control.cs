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
using Engine.WorkWithRectTransform;

namespace Engine
{
    [AddComponentMenu("Engine/Global control")]
    /// <summary>
    /// Controler all on a game scene
    /// </summary>
    public class Global_control : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI text_dialogue;
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

        [SerializeField] public SceneNow sceneNow;

        private Scenes_loader scenes_Loader;

        //for control scenes
        [SerializeField] private int sceneNumberStart = -1;
        [SerializeField] private string sceneNameStart = null;
        [SerializeField] private int sceneNumberCommandStart = 0;

        private int scene_number;
        private string scene_name;

        private int number_command_scene;

        [HideInInspector] public Dictionary<string, int> Flags;
        //

        [HideInInspector] public BackgroundsLoader backgroundsLoader;
        [HideInInspector] public SpritesLoader spritesLoader;
        [HideInInspector] public AudioLoader audioLoader;

        [SerializeField] public float speed_printing_text = 6f;

        [HideInInspector] public HandlerCommandScene handlerCommandScene;
        [HideInInspector] public ScreenshotSaverLoader screenshotSaverLoader;

        [HideInInspector] public AudioHandler audioHandler;

        [HideInInspector] public int indexPrint = 0;

        private void Awake()
        {
            if (Application.isEditor)
            {
                SaveStartSceneValues();
            }

            UpdateFiles();

            this.audioHandler = gameObject.AddComponent<AudioHandler>();

            new Save_list_names(true);

            this.screenshotSaverLoader = new ScreenshotSaverLoader();

            FindObjectOfType<SaveWindow>(true).Init();
            FindObjectOfType<LoadWindow>(true).Init();

            Flags = new Dictionary<string, int>();
            this.canvasToScreenshot.gameObject.SetActive(false);

            gameObject.AddComponent<TextPrintingClass>();

            handlerCommandScene = new HandlerCommandScene();

            foreach (Window window in FindObjectsOfType<Window>(true))
            {
                window.Init();
            }
        }

        public void SaveStartSceneValues()
        {
            SaveLoadStartScene.Save(this.sceneNumberStart, this.sceneNameStart, this.sceneNumberCommandStart);
        }

        public void UpdateFiles()
        {
            this.backgroundsLoader = new BackgroundsLoader();
            this.scenes_Loader = new Scenes_loader();
            this.spritesLoader = new SpritesLoader();
            this.audioLoader = new AudioLoader();
        }

        void Start()
        {
            this.ChangeScene(this.sceneNow.GetAllValues());

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
                if (string.IsNullOrEmpty(name))
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

            if (!this.handlerCommandScene.IsLookScene)
            {
                this.handlerCommandScene.StopLookScene(this);
            }
        }

        public void ChangeScene(Save_class saveClass)
        {
            if (saveClass.scene_number == -1)
            {
                foreach (KeyValuePair<int, Scene_class> kvp in this.scenes_Loader.Scenes_dict)
                {
                    saveClass.scene_number = kvp.Key;
                    break;
                }
            }

            if (saveClass.flags != null)
            {
                this.Flags = saveClass.flags.ConvertToDictionary();
            }
            else
            {
                this.Flags = new Dictionary<string, int>();
            }

            if (!string.IsNullOrEmpty(saveClass.nameBackground))
            {
                this.background.sprite = this.backgroundsLoader.backgrounds[saveClass.nameBackground.Split(' ')[0]];
            }

            if (saveClass.audioHelpers != null)
            {
                this.audioHandler.StopAll();
                foreach (AudioHelper.SaveClass audioHelper in saveClass.audioHelpers)
                {
                    this.audioHandler.PlayClip(this, audioHelper);
                }
            }

            if (saveClass.spritesNames != null && saveClass.spritesObjectNames != null && saveClass.rectTransformsSprites != null)
            {
                int length = Mathf.Min(saveClass.spritesNames.Length, saveClass.spritesObjectNames.Length, saveClass.rectTransformsSprites.Length);

                this.DestroyAllObjects(this.ToSpawnSprite.transform);

                for (int i = 0; i < length; i++)
                {
                    GameObject newSprite = this.SpawnObject(this.prefab_sprites, saveClass.spritesObjectNames[i], this.ToSpawnSprite.transform);
                    saveClass.rectTransformsSprites[i].UpdateRectTransform(newSprite.GetComponent<RectTransform>());
                    newSprite.GetComponent<Image>().sprite = this.spritesLoader.sprites[saveClass.spritesNames[i].Split(' ')[0]]; 
                }
            }

            this.indexPrint = saveClass.indexPrint;

            this.text_character.text = saveClass.textOnSceneCharacter;
            this.text_dialogue.text = saveClass.textOnSceneDialogue;

            TextMeshProUGUISclaeWithText.Scale(this.text_character);
            TextMeshProUGUISclaeWithText.Scale(this.text_dialogue);

            this.scene_number = saveClass.scene_number;
            this.scene_name = null;
            this.number_command_scene = saveClass.number_command_scene;

            this.SceneCommands();

            if (!this.handlerCommandScene.IsLookScene)
            {
                this.handlerCommandScene.StopLookScene(this);
            }
        }

        public void NewCommandScene()
        {

            if (this.handlerCommandScene.CanDoNextCommand())
            {
                if (gameObject.GetComponent<TextPrintingClass>().IsInit)
                {
                    gameObject.GetComponent<TextPrintingClass>().StopPrinting();
                }

                this.number_command_scene++;
                this.SceneCommands();
            }
            else if (this.handlerCommandScene.IsPrintingText)
            {
                gameObject.GetComponent<TextPrintingClass>().FinishPrinting();
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

        public GameObject SpawnObject(GameObject prefab, string name, Transform parent)
        {
            GameObject spawn_object = Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);

            if (!string.IsNullOrEmpty(name))
            {
                spawn_object.name = name;
            }

            return spawn_object;
        }

        public void DestroyObject(GameObject gameObject, float time_to_destroy = 0f)
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

        public void MakeScreenshot(string path)
        {
            this.cameraToScreenshot.gameObject.SetActive(true);
            this.canvasToScreenshot.gameObject.SetActive(true);

            foreach (GameObject to_spawn in this.RenderToScreenshot)
            {
                this.SpawnObject(to_spawn, to_spawn.transform.localPosition, to_spawn.transform.localScale, to_spawn.transform.localEulerAngles, "ToScreenshot", this.canvasToScreenshot.transform);
            }

            StartCoroutine(this.WaitScreenshot(path));
        }

        public void MakeScreenshot(string path, string nameSave)
        {
            this.cameraToScreenshot.gameObject.SetActive(true);
            this.canvasToScreenshot.gameObject.SetActive(true);

            foreach (GameObject to_spawn in this.RenderToScreenshot)
            {
                this.SpawnObject(to_spawn, to_spawn.transform.localPosition, to_spawn.transform.localScale, to_spawn.transform.localEulerAngles, "ToScreenshot", this.canvasToScreenshot.transform);
            }

            StartCoroutine(this.WaitScreenshot(path, nameSave));
        }

        IEnumerator WaitScreenshot(string path)
        {
            yield return null;

            RenderTexture texture = this.cameraToScreenshot.targetTexture;

            Texture2D texture2D = new Texture2D(1920, 1080, TextureFormat.RGB24, false);

            RenderTexture.active = texture;

            texture2D.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
            texture2D.Apply();

            File.WriteAllBytes(path, texture2D.EncodeToJPG());

            this.DestroyAllObjects(this.canvasToScreenshot.transform);

            this.cameraToScreenshot.gameObject.SetActive(false);
            this.canvasToScreenshot.gameObject.SetActive(false);

            yield break;
        }

        IEnumerator WaitScreenshot(string path, string nameSave)
        {
            yield return null;

            RenderTexture texture = this.cameraToScreenshot.targetTexture;

            Texture2D texture2D = new Texture2D(1920, 1080, TextureFormat.RGB24, false);

            RenderTexture.active = texture;

            texture2D.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
            texture2D.Apply();

            File.WriteAllBytes(path, texture2D.EncodeToJPG());

            this.DestroyAllObjects(this.canvasToScreenshot.transform);

            this.cameraToScreenshot.gameObject.SetActive(false);
            this.canvasToScreenshot.gameObject.SetActive(false);

            yield return null;

            this.screenshotSaverLoader.Update(nameSave);

            yield break;
        }

        public Pair<int, int> GetSceneValues()
        {
            return new Pair<int, int>(this.scene_number, this.number_command_scene);
        }
    }
}
