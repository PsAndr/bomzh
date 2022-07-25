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
    [AddComponentMenu("Engine/Global Control/Global Control")]
    /// <summary>
    /// Controler all on a game scene
    /// </summary>
    public class Global_control : MonoBehaviour
    {
        public enum TypeGlobalControl
        {
            Game,
            StartScene
        }
        [SerializeField, HideInInspector] public TypeGlobalControl typeGlobalControl;

        [SerializeField] public TextMeshProUGUI text_dialogue;
        [SerializeField] public TextMeshProUGUI text_character;

        [SerializeField] public GameObject canvas;

        [SerializeField] public Button ButtonScreen;

        [SerializeField] public GameObject button;
        [SerializeField] public GameObject button_field;

        [SerializeField] public GameObject ToSpawnSprite;
        [SerializeField] public Transform toSpawnVideos;

        [SerializeField] public Image background;

        [SerializeField] public GameObject prefab_sprites;

        [SerializeField] private GameObject[] RenderToScreenshot;

        [SerializeField] public GameObject[] ObjectsStopLookScene;

        [SerializeField] public SceneNow sceneNow;

        [SerializeField] public Localization localization;

        private Scenes_loader scenes_Loader;

        //for control scenes
        [SerializeField] private SceneEngine sceneStart = new();

        private int scene_number;
        private string scene_name;

        private int number_command_scene;

        [HideInInspector] public Dictionary<string, int> Flags;
        //

        [HideInInspector] public BackgroundsLoader backgroundsLoader;
        [HideInInspector] public SpritesLoader spritesLoader;
        [HideInInspector] public AudioLoader audioLoader;
        [HideInInspector] public VideoLoader videoLoader;

        [SerializeField] public SettingsGlobalControl settings;

        [HideInInspector] public HandlerCommandScene handlerCommandScene;
        [HideInInspector] public ScreenshotSaverLoader screenshotSaverLoader;

        [HideInInspector] public AudioHandler audioHandler;
        [HideInInspector] public VideoHandler videoHandler;

        [HideInInspector] public int indexPrint = 0;

        [HideInInspector] public WaitSceneCommand waitSceneCommand;

        private void Awake()
        {
            if (Application.isEditor)
            {
                SaveStartSceneValues();
            }

            UpdateFiles();

            this.audioHandler = gameObject.AddComponent<AudioHandler>();
            this.videoHandler = gameObject.AddComponent<VideoHandler>();

            new Save_list_names(true);

            this.screenshotSaverLoader = new ScreenshotSaverLoader();

            try
            {
                FindObjectOfType<SaveWindow>(true).Init();
            }
            catch { }

            try
            {
                FindObjectOfType<LoadWindow>(true).Init();
            }
            catch { }

            LocalizedTextsControl localizedTextsControl = FindObjectOfType<LocalizedTextsControl>();

            if (localizedTextsControl != null)
            {
                localizedTextsControl.CheckFileSave();
            }

            Flags = new Dictionary<string, int>();

            gameObject.AddComponent<TextPrintingClass>();

            handlerCommandScene = new HandlerCommandScene();

            this.waitSceneCommand = gameObject.AddComponent<WaitSceneCommand>();

            foreach (Window window in FindObjectsOfType<Window>(true))
            {
                window.Init();
            }

            localization.GetValuesFromSave();
        }

        public void SaveStartSceneValues()
        {
            SaveLoadStartScene.Save(this.sceneStart);
        }

        public void LoadStartSceneValues()
        {
            this.sceneStart = SaveLoadStartScene.Load();
        }

        public void UpdateFiles()
        {
            this.backgroundsLoader = new BackgroundsLoader();
            this.scenes_Loader = new Scenes_loader();
            this.spritesLoader = new SpritesLoader();
            this.audioLoader = new AudioLoader();
            this.videoLoader = new VideoLoader();
        }

        void Start()
        {
            if (this.typeGlobalControl == TypeGlobalControl.Game)
            {
                this.ChangeScene(this.sceneNow.GetAllValues());
            }
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

            if (saveClass.videoHelpers != null)
            {
                this.videoHandler.StopAll();
                foreach (VideoHelper.SaveClass videoHelper in saveClass.videoHelpers)
                {
                    this.videoHandler.PlayVideo(this, videoHelper);
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

            if (saveClass.settingsGlobalControl != null)
            {
                this.settings = saveClass.settingsGlobalControl;
            }
            else
            {

            }

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

            spawn_object.GetComponent<RectTransform>().localPosition = position;

            if (!string.IsNullOrEmpty(name))
            {
                spawn_object.name = name;
            }

            Quaternion quaternion = Quaternion.Euler(rotation);
            spawn_object.transform.localRotation = quaternion;

            spawn_object.transform.localScale = size;

            return spawn_object;
        }

        public GameObject SpawnObject(GameObject prefab, RectTransform rectTransform, string name, Transform parent)
        {
            GameObject spawn_object = Instantiate(prefab, parent);

            ((RectTransformSaveValuesSerializable)rectTransform).UpdateRectTransform(spawn_object.GetComponent<RectTransform>());

            if (!string.IsNullOrEmpty(name))
            {
                spawn_object.name = name;
            }

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

        public void MyDestroyObject(UnityEngine.Object gameObject, float time_to_destroy = 0f)
        {
            Destroy(gameObject, time_to_destroy);
        }

        public void MyDestroyObject(Transform whereObjectIs, string name)
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
            GameObject camera = new GameObject("camera___for___screenshot");
            GameObject canvas = new GameObject("canvas___for___screenshot");

            camera.SetActive(true);
            canvas.SetActive(true);

            StartCoroutine(this.WaitScreenshot(path, camera, canvas));
        }

        public void MakeScreenshot(string path, string nameSave)
        {
            GameObject camera = new GameObject("camera___for___screenshot");
            GameObject canvas = new GameObject("canvas___for___screenshot");

            camera.SetActive(true);
            canvas.SetActive(true);

            StartCoroutine(this.WaitScreenshot(path, nameSave, camera, canvas));
        }

        public void SetSceneNowValuesToStartScene()
        {
            this.sceneNow.SetDefault();
            this.sceneNow.settingsGlobalControl = this.settings;
            this.sceneNow.SetValues(this.sceneStart.sceneNumber, this.sceneStart.sceneName, this.sceneStart.numberCommandScene);
        }

        IEnumerator WaitScreenshot(string path, GameObject cameraObj, GameObject canvasObj)
        {
            Camera camera = cameraObj.AddComponent<Camera>();
            Canvas canvas = canvasObj.AddComponent<Canvas>();

            CanvasScaler canvasScaler = canvasObj.AddComponent<CanvasScaler>();
            CanvasScaler canvasScalerDefault = this.canvas.GetComponent<CanvasScaler>();

            canvasScaler.uiScaleMode = canvasScalerDefault.uiScaleMode;
            canvasScaler.referencePixelsPerUnit = canvasScalerDefault.referencePixelsPerUnit;
            canvasScaler.scaleFactor = canvasScalerDefault.scaleFactor;
            canvasScaler.referenceResolution = canvasScalerDefault.referenceResolution;
            canvasScaler.matchWidthOrHeight = canvasScalerDefault.matchWidthOrHeight;
            canvasScaler.screenMatchMode = canvasScalerDefault.screenMatchMode;

            camera.clearFlags = Camera.main.clearFlags;
            camera.cameraType = Camera.main.cameraType;
            camera.depth = Camera.main.depth;
            camera.orthographic = Camera.main.orthographic;
            camera.orthographicSize = Camera.main.orthographicSize;
            camera.farClipPlane = Camera.main.farClipPlane;
            camera.nearClipPlane = Camera.main.nearClipPlane;
            camera.nearClipPlane = Camera.main.nearClipPlane;
            camera.rect = Camera.main.rect;
            camera.depth = Camera.main.depth;

            RenderTexture texture = new RenderTexture(1920, 1080, 32);
            camera.targetTexture = texture;

            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = camera;

            foreach (GameObject to_spawn in this.RenderToScreenshot)
            {
                this.SpawnObject(to_spawn, to_spawn.GetComponent<RectTransform>(), to_spawn.name, canvas.transform);
            }

            yield return null;

            Texture2D texture2D = new Texture2D(1920, 1080, TextureFormat.RGB24, false);

            RenderTexture.active = texture;

            texture2D.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
            texture2D.Apply();

            File.WriteAllBytes(path, texture2D.EncodeToJPG());

            camera.gameObject.SetActive(false);
            canvas.gameObject.SetActive(false);

            MyDestroyObject(cameraObj);
            MyDestroyObject(canvasObj);

            yield break;
        }

        IEnumerator WaitScreenshot(string path, string nameSave, GameObject cameraObj, GameObject canvasObj)
        {
            Camera camera = cameraObj.AddComponent<Camera>();
            Canvas canvas = canvasObj.AddComponent<Canvas>();

            CanvasScaler canvasScaler = canvasObj.AddComponent<CanvasScaler>();
            CanvasScaler canvasScalerDefault = this.canvas.GetComponent<CanvasScaler>();

            canvasScaler.uiScaleMode = canvasScalerDefault.uiScaleMode;
            canvasScaler.referencePixelsPerUnit = canvasScalerDefault.referencePixelsPerUnit;
            canvasScaler.scaleFactor = canvasScalerDefault.scaleFactor;
            canvasScaler.referenceResolution = canvasScalerDefault.referenceResolution;
            canvasScaler.matchWidthOrHeight = canvasScalerDefault.matchWidthOrHeight;
            canvasScaler.screenMatchMode = canvasScalerDefault.screenMatchMode;

            camera.clearFlags = Camera.main.clearFlags;
            camera.cameraType = Camera.main.cameraType;
            camera.depth = Camera.main.depth;
            camera.orthographic = Camera.main.orthographic;
            camera.orthographicSize = Camera.main.orthographicSize;
            camera.farClipPlane = Camera.main.farClipPlane;
            camera.nearClipPlane = Camera.main.nearClipPlane;
            camera.nearClipPlane = Camera.main.nearClipPlane;
            camera.rect = Camera.main.rect;
            camera.depth = Camera.main.depth;

            RenderTexture texture = new RenderTexture(1920, 1080, 32);
            camera.targetTexture = texture;

            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = camera;

            foreach (GameObject to_spawn in this.RenderToScreenshot)
            {
                this.SpawnObject(to_spawn, to_spawn.GetComponent<RectTransform>(), to_spawn.name, canvas.transform);
            }

            yield return null;

            Texture2D texture2D = new Texture2D(1920, 1080, TextureFormat.RGB24, false);

            RenderTexture.active = texture;

            texture2D.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
            texture2D.Apply();

            File.WriteAllBytes(path, texture2D.EncodeToJPG());

            camera.gameObject.SetActive(false);
            canvas.gameObject.SetActive(false);

            yield return null;

            this.screenshotSaverLoader.Update(nameSave);

            MyDestroyObject(cameraObj);
            MyDestroyObject(canvasObj);

            yield break;
        }

        public Pair<int, int> GetSceneValues()
        {
            return new Pair<int, int>(this.scene_number, this.number_command_scene);
        }

        public T[] FindAllObjectsOfType<T>() where T : UnityEngine.Object
        {
            return FindObjectsOfType<T>(true);
        }
    }
}
