using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class SaveOverMouse
{
    [SerializeField] public GameObject coveringMouseImage_new;
    [SerializeField] public GameObject coveringMouseImage_change;

    [SerializeField] private GameObject coveringMouseChecker;

    int number;
    SaveModule save_Attach_GameObject;

    public void Init(int number, SaveModule save_Attach_GameObject)
    {
        this.number = number;
        this.save_Attach_GameObject = save_Attach_GameObject;

        if (this.coveringMouseImage_new != null) 
        {
            this.coveringMouseImage_new.SetActive(false); 
        }
        if (this.coveringMouseImage_change != null)
        {
            this.coveringMouseImage_change.SetActive(false);
        }
        if (this.coveringMouseChecker != null)
        {
            this.coveringMouseChecker.SetActive(true);
            this.coveringMouseChecker.AddComponent<SaveOverMouseBehaviour>();
            this.coveringMouseChecker.GetComponent<SaveOverMouseBehaviour>().parentScript = this;
        }
    }

    public void Enter()
    {
        save_Attach_GameObject.Enter(this.number);
    }

    public void Exit()
    {
        save_Attach_GameObject.Exit(this.number);
    }
}
