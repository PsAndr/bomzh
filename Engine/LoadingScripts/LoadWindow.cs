using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Engine
{
    /// <summary>
    /// Load window controller
    /// </summary>
    [AddComponentMenu("Engine/Load/LoadWindow")]
    public class LoadWindow : MonoBehaviour
    {
        [SerializeField] private bool startScene;
        [SerializeField] private Global_control global_Control;
        [SerializeField] private SceneUnityController sceneUnityController;

        [SerializeField] private GameObject prefabModule;

        [SerializeField] private int countLines;
        [SerializeField] private float deltaHorizontal;

        [SerializeField] private GameObject toSpawnModules;

        [HideInInspector] public string newSceneStart;
        [HideInInspector] public int sceneIndex;

        private bool IsInit = false;

        /// <summary>
        /// Start load window
        /// </summary>
        public void Init()
        {
            if (this.toSpawnModules == null)
            {
                this.toSpawnModules = gameObject;
            }

            if (this.global_Control == null)
            {
                this.global_Control = FindObjectOfType<Global_control>();
            }

            if (this.sceneUnityController == null)
            {
                this.sceneUnityController = FindObjectOfType<SceneUnityController>();
            }

            //this.LoadSaves();

            this.IsInit = true;
        }

        IEnumerator WaitNextFrame()
        {
            yield return null;
            this.IsInit = true;
            yield break;
        }

        IEnumerator WaitCheckScale()
        {
            yield return null;
            this.CheckScale();
            yield break;
        }

        private void LoadSaves()
        {
            Save_list_names save_List = new Save_list_names();

            int lineIndex = 0;
            int columnIndex = 0;
            int index = 0;

            float width = this.toSpawnModules.GetComponent<RectTransform>().sizeDelta.x;
            float height = this.toSpawnModules.GetComponent<RectTransform>().sizeDelta.y;

            float deltaVertical = height / this.countLines;

            foreach (string name in save_List.list_names)
            {
                GameObject spawnObj = Global_control.SpawnObject(this.prefabModule,
                    new Vector3(this.deltaHorizontal / 2 + this.deltaHorizontal * columnIndex, -((deltaVertical / 2) + lineIndex * deltaVertical)),
                        new Vector3(1, 1, 1), new Vector3(), name, this.toSpawnModules.transform);

                spawnObj.SetActive(true);
                spawnObj.GetComponent<LoadModule>().Init(name);
                spawnObj.GetComponent<LoadModule>().loadWindow = this;

                index++;
                lineIndex++;

                columnIndex += lineIndex / this.countLines;
                lineIndex %= this.countLines;
            }

            this.CheckScale();
        }

        private void CheckScale()
        {
            LoadModule[] loadModules = this.toSpawnModules.transform.GetComponentsInChildren<LoadModule>();
            int countModules = loadModules.Length;

            int countModulesLine = (countModules - 1) / this.countLines + 1;

            float height = this.toSpawnModules.GetComponent<RectTransform>().sizeDelta.y;
            this.toSpawnModules.GetComponent<RectTransform>().sizeDelta = new Vector2(this.deltaHorizontal * countModulesLine, height);

            this.UpdatePositionsModules();
        }

        public void UpdatePositionsModules()
        {
            LoadModule[] loadModules = this.toSpawnModules.transform.GetComponentsInChildren<LoadModule>();

            int lineIndex = 0;
            int columnIndex = 0;

            float height = this.toSpawnModules.GetComponent<RectTransform>().sizeDelta.y;

            float deltaVertical = height / this.countLines;

            foreach (LoadModule loadModule in loadModules)
            {
                GameObject obj = loadModule.gameObject;
                obj.transform.localPosition = new Vector3(this.deltaHorizontal / 2 + this.deltaHorizontal * columnIndex, -((deltaVertical / 2) + lineIndex * deltaVertical));

                lineIndex++;

                columnIndex += lineIndex / this.countLines;
                lineIndex %= this.countLines;
            }
        }

        public void Load(string nameSave)
        {
            Save_class saveLoad = Save_class.Load(nameSave);
            if (this.startScene)
            {
                print(saveLoad.videoHelpers.Length);
                this.global_Control.sceneNow.SetValues(saveLoad);
                sceneUnityController.LoadNewScene(this.newSceneStart);
            }
            else
            {
                this.global_Control.ChangeScene(saveLoad);
            }
        }

        private void OnEnable()
        {
            if (this.IsInit)
            {
                this.LoadSaves();
            }
        }

        private void OnDisable()
        {
            if (this.IsInit)
            {
                LoadModule[] loadModules = this.toSpawnModules.transform.GetComponentsInChildren<LoadModule>();

                foreach (LoadModule loadModule in loadModules)
                {
                    GameObject obj = loadModule.gameObject;
                    this.global_Control.MyDestroyObject(obj);
                }
            }
        }
    }
}
