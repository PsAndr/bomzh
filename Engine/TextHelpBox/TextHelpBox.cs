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

        [SerializeField] private Image background;

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

            if (background == null)
            {
                background = GetNearHierarchyObjectOfType<Image>.GetWithoutException(gameObject.transform);
            }

            GetPositionsOfSymbolsInTMPRO.SetObjectPositionAndSizeOverText(textOver, gameObject.GetComponent<RectTransform>(), indexStart, indexEnd);
            Hide();

            SpawnLines();
            background.gameObject.SetActive(false);

            needShowOrHideBuffer = StateBox.None;
        }

        private void SpawnLines()
        {
            GameObject toSpawnBackgrounds = gameObject.transform.GetChild(0).gameObject;

            if (toSpawnBackgrounds.name != "___toSpawnBackgrounds___")
            {
                toSpawnBackgrounds = new GameObject("___toSpawnBackgrounds___");
                toSpawnBackgrounds.transform.SetParent(gameObject.transform);
                toSpawnBackgrounds.transform.SetSiblingIndex(0);

                toSpawnBackgrounds.AddComponent<RectMask2D>();
                toSpawnBackgrounds.AddComponent<RectTransform>();
            }

            Global_control.DestroyAllObjects(toSpawnBackgrounds.transform);

            Pair<Vector3, Vector3>[] lines = GetPositionsOfSymbolsInTMPRO.GetLines(textOver, indexStart, indexEnd);

            List<GameObject> linesObjects = new();

            for (int indexLine = 0; indexLine < lines.Length; indexLine++)
            {
                GameObject lineObject = new GameObject($"__lineToSpawnBackGroundsNumber:{indexLine}___");

                lineObject.AddComponent<RectTransform>();
                lineObject.AddComponent<RectMask2D>();

                lineObject.transform.SetParent(toSpawnBackgrounds.transform);

                RectTransform lineObjectRect = lineObject.GetComponent<RectTransform>();

                lineObjectRect.anchorMin = new Vector2(0f, 1f);
                lineObjectRect.anchorMax = new Vector2(0f, 1f);

                lineObjectRect.pivot = new Vector2(0f, 1f);

                lineObjectRect.anchoredPosition3D = lines[indexLine].first - lines[0].first;

                lineObjectRect.anchoredPosition3D = new Vector3(0f, lineObjectRect.anchoredPosition3D.y, 0f);

                lineObjectRect.sizeDelta = new Vector2
                (
                    Mathf.Abs(lines[indexLine].first.x - lines[indexLine].second.x), Mathf.Abs(lines[indexLine].first.y - lines[indexLine].second.y)
                );

                linesObjects.Add(lineObject);
            }

            toSpawnBackgrounds.SetActive(true);
            RectTransform toSpawnBackgroundsRect = toSpawnBackgrounds.GetComponent<RectTransform>();

            toSpawnBackgroundsRect.anchorMin = Vector2.zero;
            toSpawnBackgroundsRect.anchorMax = Vector2.one;

            toSpawnBackgroundsRect.sizeDelta = Vector2.zero;
            toSpawnBackgroundsRect.anchoredPosition3D = Vector3.zero;

            toSpawnBackgroundsRect.localScale = Vector3.one;

            SpawnBackgrounds(linesObjects.ToArray());
        }

        private void SpawnBackgrounds(GameObject[] lines)
        {
            foreach (GameObject line in lines) 
            {
                Vector2 sizeDeltaLine = line.GetComponent<RectTransform>().sizeDelta;
                Vector2 sizeDeltaBackground = background.GetComponent<RectTransform>().sizeDelta;

                int cntLines = (int)(sizeDeltaLine.y / sizeDeltaBackground.y) + (sizeDeltaLine.y % sizeDeltaBackground.y > 0f ? 1 : 0);
                int cntRows = (int)(sizeDeltaLine.x / sizeDeltaBackground.x) + (sizeDeltaLine.x % sizeDeltaBackground.x > 0f ? 1 : 0);

                for (int indexLine = 0; indexLine < cntLines; indexLine++)
                {
                    for (int indexRow = 0; indexRow < cntRows; indexRow++)
                    {
                        Vector3 position = Vector3.zero;
                        position.x = indexRow * sizeDeltaBackground.x;
                        position.y = -indexLine * sizeDeltaBackground.y;

                        GameObject newBackground = Instantiate(background.gameObject, line.transform);
                        RectTransform newBackgroundRect = newBackground.GetComponent<RectTransform>();

                        newBackgroundRect.anchorMin = new Vector2(0f, 1f);
                        newBackgroundRect.anchorMax = new Vector2(0f, 1f);
                        newBackgroundRect.pivot = new Vector2(0f, 1f);

                        newBackgroundRect.anchoredPosition3D = position;
                    }
                }
            }
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

            Init();
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
