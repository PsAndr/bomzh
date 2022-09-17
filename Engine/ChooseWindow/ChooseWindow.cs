using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace Engine
{
    [AddComponentMenu("Engine/Choose Window/Choose Window")]
    public class ChooseWindow : MonoBehaviour
    {
        private UnityEvent<int> chooseEvent = new UnityEvent<int>();

        [SerializeField] private GameObject item;

        [SerializeField] private RectTransform content;
        [SerializeField] private RectTransform[] scaleWithScreen;

        [SerializeField] private bool scaleContent;

        [SerializeField] private Button applyButton;
        [SerializeField] private Button backButton;

        [SerializeField] private TextMeshProUGUI nameWindow;

        [SerializeField] private Global_control global_Control;

        private List<ChooseWindowItem> items = new();

        private int index;

        private float deltaY;

        private GameObject stopOtherUI;

        public void Init(UnityAction<int> action, int indexSelect, float deltaY, string nameWindow, params string[] selectionOptions)
        {
            this.index = indexSelect;
            this.deltaY = deltaY;

            this.nameWindow.text = nameWindow;

            chooseEvent.AddListener(action);

            applyButton.onClick.AddListener(this.OnApplyClick);
            backButton.onClick.AddListener(this.CloseWindow);

            if (global_Control == null)
            {
                global_Control = FindObjectOfType<Global_control>();
            }

            item.SetActive(false);

            this.SpawnItems(selectionOptions);
            this.SpawnStopOtherUI();

            RectTransform canvas = transform.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

            foreach (RectTransform rectTransform in this.scaleWithScreen)
            {
                rectTransform.localScale = Vector3.one;
                rectTransform.anchorMin = new Vector3(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector3(0.5f, 0.5f);
                rectTransform.position = canvas.position;
                rectTransform.sizeDelta = canvas.sizeDelta;
            }
        }

        private void SpawnStopOtherUI()
        {
            this.stopOtherUI = new GameObject();
            this.stopOtherUI.AddComponent<RectTransform>();
            this.stopOtherUI.AddComponent<Image>().color = Color.clear;
            this.stopOtherUI.name = "stopOtherUI";

            this.stopOtherUI.transform.SetParent(transform.parent, true);
            this.stopOtherUI.GetComponent<RectTransform>().anchorMax = Vector2.one;
            this.stopOtherUI.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            this.stopOtherUI.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            this.stopOtherUI.transform.localPosition = Vector3.zero;
            this.stopOtherUI.GetComponent<RectTransform>().localScale = Vector3.one;
            this.stopOtherUI.transform.SetSiblingIndex(transform.GetSiblingIndex());
        }

        private void SpawnItems(params string[] selectionOptions)
        {
            this.item.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);

            RectTransform rectTransform = item.GetComponent<RectTransform>();

            if (this.scaleContent)
            {
                this.content.sizeDelta = new Vector2(this.content.sizeDelta.x, rectTransform.sizeDelta.y * selectionOptions.Length + this.deltaY * selectionOptions.Length);
            }

            int i = 0;
            foreach (string selectionOption in selectionOptions)
            {
                GameObject newItem = Global_control.SpawnObject(this.item, new Vector3(0f, 
                    -i * rectTransform.sizeDelta.y - this.deltaY * (i + 1) + this.content.sizeDelta.y / 2f, 0f),
                    Vector3.one, Vector3.zero, i.ToString(), content);

                newItem.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f,
                    -i * rectTransform.sizeDelta.y - this.deltaY * (i + 0.5f) + this.content.sizeDelta.y / 2f, 0f);
                newItem.SetActive(true);
                newItem.GetComponentInChildren<TextMeshProUGUI>().text = selectionOption;

                newItem.GetComponent<ChooseWindowItem>().Init(this);

                if (i == this.index)
                {
                    newItem.GetComponent<ChooseWindowItem>().SetSelected();
                }

                this.items.Add(newItem.GetComponent<ChooseWindowItem>());

                i++;
            }
        }

        private void CloseWindow()
        {
            Destroy(this.stopOtherUI);
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

        public void ChangeIndex(int newIndex)
        {
            this.items[this.index].SetDeselected();
            this.items[newIndex].SetSelected();
            this.index = newIndex;
        }
    }
}
