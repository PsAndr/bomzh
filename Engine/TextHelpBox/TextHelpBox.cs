using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Engine.WorkWithTextMeshPro;
using TMPro;
using System.Threading;

namespace Engine
{
    [AddComponentMenu("Engine/Text Help Box/Text Help Box")]
    public class TextHelpBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeField] private TextMeshProUGUI textOver;

        [SerializeField] private GameObject box;
        [SerializeField] private TextMeshProUGUI textOfBox;

        [SerializeField] private int indexStart;
        [SerializeField] private int indexEnd;

        private enum StateBox
        {
            Open = 1,
            Close = -1,
            None = 0,
        }

        private StateBox needShowOrHideBuffer = StateBox.None;

        public void Init(string textOfBox, int indexStart, int indexEnd)
        {
            Init();

            Change(textOfBox, indexStart, indexEnd);
        }

        public void Init()
        {
            if (textOver == null)
            {
                textOver = GetNearHierarchyObjectOfType<TextMeshProUGUI>.GetWithoutException(gameObject.transform.parent);
            }

            if (box == null)
            {
                box = gameObject;
            }

            if (textOfBox == null)
            {
                textOfBox = GetNearHierarchyObjectOfType<TextMeshProUGUI>.GetWithoutException(box.transform);
            }

            GetPositionsOfSymbolsInTMPRO.SetObjectPositionAndSizeOverText(textOver, gameObject.GetComponent<RectTransform>(), indexStart, indexEnd);
            Hide();

            needShowOrHideBuffer = StateBox.None;
        }

        private void Start()
        {
            StartCoroutine(InitAfterOneFrame());
        }

        private IEnumerator InitAfterOneFrame()
        {
            yield return null;
            Init();
            yield break;
        }

        public void Change(string textOfBox, int indexStart, int indexEnd)
        {
            this.textOfBox.text = textOfBox;
            this.indexStart = indexStart;
            this.indexEnd = indexEnd;
        }

        public void Show()
        {
            box.SetActive(true);
        }

        public void Hide()
        {
            box.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
#if UNITY_EDITOR || UNITY_STANDALONE 
            needShowOrHideBuffer = StateBox.Open;
#endif
        }

        public void OnPointerExit(PointerEventData eventData)
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            needShowOrHideBuffer = StateBox.Close;
#endif
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            needShowOrHideBuffer = StateBox.Open;
        }

        public void CheckMouseClick()
        {
            if (Event.current.type == EventType.MouseDown && needShowOrHideBuffer != StateBox.Open)
            {
                needShowOrHideBuffer = StateBox.Close;
            }
        }

        private void OnGUI()
        {
            CheckMouseClick();
        }

        private void Update()
        {
            switch (needShowOrHideBuffer)
            {
                case StateBox.Close:
                    needShowOrHideBuffer = StateBox.None;
                    Hide();
                    break;

                case StateBox.Open:
                    needShowOrHideBuffer = StateBox.None;
                    Show();
                    break;
            }
        }
    }
}
