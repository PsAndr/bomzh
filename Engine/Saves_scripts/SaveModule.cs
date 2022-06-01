using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

[Serializable]
public class SaveModule : MonoBehaviour
{
    [SerializeField] private Global_control global_Control;

    [SerializeField] private TMP_InputField name_save;

    [SerializeField] private SaveOverMouse[] overMouse;

    [SerializeField] private Button button;
    [SerializeField] private Button startChangeName;

    [HideInInspector] public bool IsNew;

    [HideInInspector] public int numberSave;

    [HideInInspector] public string nameSave = "Имя сохранения";

    void Start()
    {
        this.name_save.text = this.nameSave;
        this.IsNew = false;
        
        this.button.onClick.AddListener(OnClick);

        this.name_save.onValueChanged.AddListener(this.NameChange);
        this.name_save.onDeselect.AddListener(this.DeselectName);
        this.name_save.onEndEdit.AddListener(this.EndChangeName);

        if (this.startChangeName == null)
        {
            this.name_save.readOnly = false;
        }
        else
        {
            this.startChangeName.onClick.AddListener(CanChangeName);
            this.name_save.readOnly = true;
        }

        int i = 0;
        foreach (SaveOverMouse saveOverMouse in this.overMouse)
        {
            saveOverMouse.Init(i, this);
            i++;
        }
    }

    /*private void OnGUI()
    {
        //Debug.Log(Event.current.mousePosition.ToString());
    }*/

    void Update()
    {

    }

    public void Enter(int num)
    {
        if (this.IsNew)
        {
            if (this.overMouse[num].coveringMouseImage_new == null)
            {
                return;
            }
            this.overMouse[num].coveringMouseImage_new.SetActive(true);
        }
        else
        {
            if (this.overMouse[num].coveringMouseImage_change == null)
            {
                return;
            }
            this.overMouse[num].coveringMouseImage_change.SetActive(true);
        }
    }

    public void Exit(int num)
    {
        if (this.IsNew)
        {
            if (this.overMouse[num].coveringMouseImage_new == null)
            {
                return;
            }
            this.overMouse[num].coveringMouseImage_new.SetActive(false);
        }
        else
        {
            if (this.overMouse[num].coveringMouseImage_change == null)
            {
                return;
            }
            this.overMouse[num].coveringMouseImage_change.SetActive(false);
        }
    }

    private void OnClick()
    {
        if (this.IsNew)
        {

        }
        else
        {
            new Save_class(this.name_save.text).Change(this.global_Control.GetSceneValues().first, this.global_Control.GetSceneValues().second, this.global_Control.Flags);
            Debug.Log(Application.persistentDataPath);
        }
    }

    private void CanChangeName()
    {
        EventSystem.current.SetSelectedGameObject(name_save.gameObject);
        this.name_save.readOnly = false;
    }

    private void NameChange(string s)
    {

    }

    private void DeselectName(string s)
    {

    }

    private void EndChangeName(string s)
    {
        new Save_class(this.nameSave).Change(s);
        this.nameSave = s;
        if (this.startChangeName != null)
        {
            this.name_save.readOnly = true;
        }
    }
}
