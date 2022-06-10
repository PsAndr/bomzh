using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Engine
{
    [System.Serializable]
    public class SaveOverMouse
    {
        [SerializeField] public GameObject coveringMouseImage_new;
        [SerializeField] public GameObject coveringMouseImage_change;

        [SerializeField] private GameObject coveringMouseChecker;

        int number;
        SaveModule save_Attach_GameObject;
        [HideInInspector] public bool IsActive;

        public void Init(int number, SaveModule save_Attach_GameObject)
        {
            this.number = number;
            this.save_Attach_GameObject = save_Attach_GameObject;
            this.IsActive = false;

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
            this.IsActive = true;
            save_Attach_GameObject.Enter(this.number);
        }

        public void Exit()
        {
            this.IsActive = false;
            save_Attach_GameObject.Exit(this.number);
        }
    }
}
