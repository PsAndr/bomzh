using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[SerializeField]
public class LoadOverMouse
{
    [SerializeField] public GameObject coveringMouseImage;
    [SerializeField] public GameObject coveringMouseChecker;

    [HideInInspector] public bool IsActive;

    public void Init()
    {
        this.IsActive = false;

        if (this.coveringMouseImage != null)
        {
            this.coveringMouseImage.SetActive(false);
        }
        if (this.coveringMouseChecker != null)
        {
            this.coveringMouseChecker.SetActive(true);
            this.coveringMouseChecker.AddComponent<LoadOverMouseBehaviour>().parentScript = this;
        }
    }

    public void Enter()
    {
        this.IsActive = true;
        if (this.coveringMouseImage != null)
        {
            this.coveringMouseImage.SetActive(true);
        }
    }

    public void Exit()
    {
        this.IsActive = false;
        if (this.coveringMouseImage != null)
        {
            this.coveringMouseImage.SetActive(false);
        }
    }
}
