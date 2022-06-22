using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Engine
{
    public class ChooseOpenButton : MonoBehaviour
    {
        [SerializeField] private ChooseWindow chooseWindow;

        [SerializeField] private Object valuesGetFrom;

        [SerializeField] private string nameWindow;

        public void OpenChooseWindow()
        {
            ChooseHelperClass chooseHelper = (ChooseHelperClass)valuesGetFrom;

            ChooseWindow chooseWindow = FindObjectOfType<Global_control>().SpawnObject(this.chooseWindow.gameObject, 
                Vector3.zero, Vector3.one, Vector3.zero, null, 
                gameObject.GetComponentInParent<Canvas>().transform).GetComponent<ChooseWindow>();
            chooseWindow.Init(this.ChangeValue, chooseHelper.GetIndex(), 100, this.nameWindow, chooseHelper.GetSelectionOptions());
        }

        private void ChangeValue(int index)
        {
            ChooseHelperClass chooseHelper = (ChooseHelperClass)this.valuesGetFrom;
            chooseHelper.ChangeIndex(index);

            chooseHelper.PasteValues(ref this.valuesGetFrom);
        }
    }
}
