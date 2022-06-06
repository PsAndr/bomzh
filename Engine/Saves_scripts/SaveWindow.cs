using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[AddComponentMenu("Engine/Save/Window")]
public class SaveWindow : MonoBehaviour
{
    [SerializeField] GameObject prefabModule;

    [SerializeField] int countLines;
    [SerializeField] int countInLine;

    [SerializeField] float deltaHorizontal;

    [SerializeField] Global_control global_Control;

    [SerializeField] GameObject toSpawnModules;
    [SerializeField] bool scaleWithSavesCount;

    public void Init()
    {
        if (this.global_Control == null)
        {
            this.global_Control = FindObjectOfType<Global_control>();
        }

        if (this.toSpawnModules == null)
        {
            this.toSpawnModules = gameObject;
        }
        this.LoadSaves();
    }

    private void Awake()
    {
        
    }

    private void Start()
    {
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
            GameObject spawnObj = this.global_Control.SpawnObject(this.prefabModule,
                new Vector3(this.deltaHorizontal / 2 + this.deltaHorizontal * columnIndex, -((deltaVertical / 2) + lineIndex * deltaVertical)),
                    new Vector3(1, 1, 1), new Vector3(), name, this.toSpawnModules.transform);

            spawnObj.SetActive(true);
            spawnObj.GetComponent<SaveModule>().nameSave = name;
            spawnObj.GetComponent<SaveModule>().numberSave = index;
            spawnObj.GetComponent<SaveModule>().IsNew = false;
            spawnObj.GetComponent<SaveModule>().saveWindow = this;
            spawnObj.GetComponent<SaveModule>().Init();

            index++;
            lineIndex++;

            columnIndex += lineIndex / this.countLines;
            lineIndex %= this.countLines;
        }

        this.SpawnNewSave();

        CheckScale();
    }

    public void SpawnNewSave()
    {
        SaveModule[] saveModules = this.toSpawnModules.transform.GetComponentsInChildren<SaveModule>();
        int countModules = saveModules.Length + 1;

        if (countModules > this.countLines * this.countInLine)
        {
            return;
        }

        int lineIndex = (countModules - 1) % this.countLines;
        int columnIndex = (countModules - 1) / this.countLines;
        int index = countModules - 1;

        float width = this.toSpawnModules.GetComponent<RectTransform>().sizeDelta.x;
        float height = this.toSpawnModules.GetComponent<RectTransform>().sizeDelta.y;

        float deltaVertical = height / this.countLines;

        GameObject spawnObj = this.global_Control.SpawnObject(this.prefabModule,
            new Vector3(this.deltaHorizontal / 2 + this.deltaHorizontal * columnIndex, -((deltaVertical / 2) + lineIndex * deltaVertical)),
                new Vector3(1, 1, 1), new Vector3(), "new save", this.toSpawnModules.transform);

        spawnObj.SetActive(true);
        spawnObj.GetComponent<SaveModule>().nameSave = "Новое сохранение";
        spawnObj.GetComponent<SaveModule>().numberSave = index;
        spawnObj.GetComponent<SaveModule>().IsNew = true;
        spawnObj.GetComponent<SaveModule>().saveWindow = this;
        spawnObj.GetComponent<SaveModule>().Init();

        this.CheckScale(columnIndex + 1);
    }

    private void CheckScale(int countModulesLine)
    {
        float height = this.toSpawnModules.GetComponent<RectTransform>().sizeDelta.y;
        this.toSpawnModules.GetComponent<RectTransform>().sizeDelta = new Vector2(this.deltaHorizontal * countModulesLine, height);

        this.UpdatePositionsModules();
    }

    private void CheckScale()
    {
        SaveModule[] saveModules = this.toSpawnModules.transform.GetComponentsInChildren<SaveModule>();
        int countModules = saveModules.Length;

        int countModulesLine = (countModules - 1) / this.countLines + 1;

        float height = this.toSpawnModules.GetComponent<RectTransform>().sizeDelta.y;
        this.toSpawnModules.GetComponent<RectTransform>().sizeDelta = new Vector2(this.deltaHorizontal * countModulesLine, height);

        this.UpdatePositionsModules();
    }

    public void UpdatePositionsModules()
    {
        SaveModule[] saveModules = this.toSpawnModules.transform.GetComponentsInChildren<SaveModule>();

        int lineIndex = 0;
        int columnIndex = 0;

        float height = this.toSpawnModules.GetComponent<RectTransform>().sizeDelta.y;

        float deltaVertical = height / this.countLines;

        foreach (SaveModule saveModule in saveModules)
        {
            GameObject obj = saveModule.gameObject;
            obj.transform.localPosition = new Vector3(this.deltaHorizontal / 2 + this.deltaHorizontal * columnIndex, -((deltaVertical / 2) + lineIndex * deltaVertical));

            lineIndex++;

            columnIndex += lineIndex / this.countLines;
            lineIndex %= this.countLines;
        }
    }

    public void StartWaitDeleteModule()
    {
        StartCoroutine(this.WaitDeleteModule());
    }

    IEnumerator WaitDeleteModule()
    {
        yield return null;
        this.CheckScale();
        yield break;
    }
}
