using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace Engine
{
    public class ChooseWindow : MonoBehaviour
    {
        private UnityEvent<int> chooseEvent = new UnityEvent<int>();

        [SerializeField] private GameObject item;

        [SerializeField] private RectTransform content;

        [SerializeField] private Global_control global_Control;

        private int index;

        private float deltaY;

        public void Init(UnityAction<int> action, int indexSelect, float deltaY, params string[] selectionOptions)
        {
            this.index = indexSelect;
            this.deltaY = deltaY;

            chooseEvent.AddListener(action);

            if (global_Control == null)
            {
                global_Control = FindObjectOfType<Global_control>();
            }

            item.SetActive(false);

            this.SpawnItems(selectionOptions);
        }

        private void SpawnItems(params string[] selectionOptions)
        {
            this.item.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);

            RectTransform rectTransform = item.GetComponent<RectTransform>();

            int i = 0;
            foreach (string selectionOption in selectionOptions)
            {
                GameObject newItem = this.global_Control.SpawnObject(this.item, new Vector3(-rectTransform.sizeDelta.x / 2f, 
                    -i * rectTransform.sizeDelta.y - this.deltaY * i + this.content.sizeDelta.y / 2f, 0f),
                    Vector3.one, Vector3.zero, i.ToString(), this.gameObject.transform);

                newItem.SetActive(true);
                newItem.GetComponentInChildren<TextMeshProUGUI>().text = selectionOption;

                i++;
            }
        }

        private void CloseWindow()
        {
            Destroy(gameObject);
        }

        public void OnApplyClick()
        {
            chooseEvent.Invoke(this.index);
            this.CloseWindow();
        }

        public GameObject GetItem()
        {
            return item;
        }
    }
}
