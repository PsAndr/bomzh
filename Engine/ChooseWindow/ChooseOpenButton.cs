using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class ChooseOpenButton : MonoBehaviour
    {
        [SerializeField] private ChooseWindow chooseWindow;

        public void OpenChooseWindow()
        {
            ChooseWindow chooseWindow = FindObjectOfType<Global_control>().SpawnObject(this.chooseWindow.gameObject, 
                Vector3.zero, Vector3.one, Vector3.zero, null, 
                gameObject.GetComponentInParent<Canvas>().transform).GetComponent<ChooseWindow>();
            chooseWindow.Init(this.ChangeValue, 0, 25, "1", "2", "3");
        }

        private void ChangeValue(int index)
        {
            Debug.Log(index);
        }
    }
}
