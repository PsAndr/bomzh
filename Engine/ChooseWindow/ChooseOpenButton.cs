using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Engine
{
    [AddComponentMenu("Engine/Choose Window/Open Button")]
    public class ChooseOpenButton : MonoBehaviour
    {
        [System.Serializable]
        public enum TypeChooseOpenButton
        {
            ValuesFromObject,
            TypesSkiping
        }

        [SerializeField] private ChooseWindow chooseWindow;
        
        [SerializeField, HideInInspector] private Object valuesGetFrom;

        [SerializeField] private TextMeshProUGUI nameWindow;

        [SerializeField] public TypeChooseOpenButton typeChooseOpenButton;

        public void OpenChooseWindow()
        {
            ChooseHelperClass chooseHelper = null;

            switch (typeChooseOpenButton)
            {
                case TypeChooseOpenButton.ValuesFromObject:
                    chooseHelper = (ChooseHelperClass)valuesGetFrom;
                    break;

                case TypeChooseOpenButton.TypesSkiping:
                    chooseHelper = (ChooseHelperClass)FindObjectOfType<Global_control>().settings.TypeSkiping;
                    break;

                default:
                    return;
            }

            ChooseWindow chooseWindow = FindObjectOfType<Global_control>().SpawnObject(this.chooseWindow.gameObject, 
                Vector3.zero, Vector3.one, Vector3.zero, null, 
                gameObject.GetComponentInParent<Canvas>().transform).GetComponent<ChooseWindow>();
            chooseWindow.Init(this.ChangeValue, chooseHelper.GetIndex(), 100, this.nameWindow.text, chooseHelper.GetSelectionOptions());
        }

        private void ChangeValue(int index)
        {
            ChooseHelperClass chooseHelper = null;

            switch (typeChooseOpenButton)
            {
                case TypeChooseOpenButton.ValuesFromObject:
                    chooseHelper = (ChooseHelperClass)valuesGetFrom;

                    chooseHelper.ChangeIndex(index);

                    chooseHelper.PasteValues(ref this.valuesGetFrom);
                    break;

                case TypeChooseOpenButton.TypesSkiping:
                    FindObjectOfType<Global_control>().settings.TypeSkiping = (TypesSkiping)index;
                    break;

                default:
                    return;
            }
        }
    }
}
