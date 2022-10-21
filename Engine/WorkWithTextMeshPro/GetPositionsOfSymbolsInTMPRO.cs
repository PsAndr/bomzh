using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Engine;
using UnityEngine.UI;
using System;

namespace Engine.WorkWithTextMeshPro
{
    public static class GetPositionsOfSymbolsInTMPRO
    {
        /// <summary>
        /// Get positions
        /// </summary>
        /// <param name="text">TextMeshProUGUI where is needed symbols</param>
        /// <param name="indexStart">start index of string`s part</param>
        /// <param name="indexEnd">last index of string`s part</param>
        /// <returns>Pair(Vector3, Vector3); First: top left position, Second: bottom right position</returns>
        public static Pair<Vector3, Vector3> Get(TextMeshProUGUI text, int indexStart = 0, int indexEnd = -1)
        {
            if (indexEnd == -1)
            {
                indexEnd = text.text.Length - 1;
            }

            if (indexStart > indexEnd)
            {
                SwapClass.Swap(ref indexStart, ref indexEnd);
            }

            if (indexStart < 0 || indexEnd >= text.textInfo.characterInfo.Length)
            {
                throw new Exception(
                    $"class: GetPositionsOfSymbolsInTMPRO; function: Get; Wrong indexes!; " +
                    $"indexStart: {indexStart}; indexEnd: {indexEnd}; " +
                    $"length text: {text.textInfo.characterInfo.Length}; text: {text.text};"
                );
            }

            Vector3 topLeft = text.textInfo.characterInfo[indexStart].topLeft;
            Vector3 bottomRight = text.textInfo.characterInfo[indexEnd].bottomRight;

            for (int indexCharacter = indexStart; indexCharacter <= indexEnd; indexCharacter++)
            {
                topLeft = new Vector3
                (
                    Mathf.Min(text.textInfo.characterInfo[indexCharacter].topLeft.x, topLeft.x),
                    Mathf.Max(text.textInfo.characterInfo[indexCharacter].topLeft.y, topLeft.y),
                    topLeft.z
                );
                bottomRight = new Vector3
                (
                    Mathf.Max(text.textInfo.characterInfo[indexCharacter].bottomRight.x, bottomRight.x),
                    Mathf.Min(text.textInfo.characterInfo[indexCharacter].bottomRight.y, bottomRight.y),
                    topLeft.z
                );
            }

            return (text.transform.localPosition + topLeft, text.transform.localPosition + bottomRight);
        }

        /// <summary>
        /// Get positions lines
        /// </summary>
        /// <param name="text">TextMeshProUGUI where is needed symbols</param>
        /// <param name="indexStart">start index of string`s part</param>
        /// <param name="indexEnd">last index of string`s part</param>
        /// <returns>Pair(Vector3, Vector3)[]; (First: top left position, Second: bottom right position) for lines</returns>
        public static Pair<Vector3, Vector3>[] GetLines(TextMeshProUGUI text, int indexStart = 0, int indexEnd = -1)
        {
            if (indexEnd == -1)
            {
                indexEnd = text.text.Length - 1;
            }

            if (indexStart > indexEnd)
            {
                SwapClass.Swap(ref indexStart, ref indexEnd);
            }

            if (indexStart < 0 || indexEnd >= text.textInfo.characterInfo.Length)
            {
                throw new System.Exception(
                    $"class: GetPositionsOfSymbolsInTMPRO; function: GetLines; Wrong indexes!; " +
                    $"indexStart: {indexStart}; indexEnd: {indexEnd}"
                );
            }

            List<Pair<Vector3, Vector3>> lines = new List<Pair<Vector3, Vector3>>();

            int indexFirstInLine = indexStart;
            int indexLine = text.textInfo.characterInfo[indexStart].lineNumber;

            for (int index = indexStart + 1; index <= indexEnd; index++)
            {
                if (text.textInfo.characterInfo[index].lineNumber > indexLine)
                {
                    lines.Add(Get(text, indexFirstInLine, index - 1));
                    indexLine = text.textInfo.characterInfo[index].lineNumber;
                    indexFirstInLine = index;
                }
            }

            lines.Add(Get(text, indexFirstInLine, indexEnd));

            return lines.ToArray();
        }

        /// <summary>
        /// Set position of object, to cover part of text
        /// </summary>
        /// <param name="text">text to cover</param>
        /// <param name="obj">object to set position</param>
        /// <param name="indexStart">start index of part</param>
        /// <param name="indexEnd">end index of part</param>
        public static void SetObjectPositionAndSizeOverText(TextMeshProUGUI text, RectTransform obj, int indexStart = 0, int indexEnd = -1)
        {
            if (obj == null)
            {
                throw new System.Exception
                (
                    $"class: GetPositionsOfSymbolsInTMPRO; function: SetObjectPositionAndSizeOverText; Wrong obj!; " +
                    $"obj == null"
                );
            }

            Vector3 topLeft, bottomRight;

            (topLeft, bottomRight) = Get(text, indexStart, indexEnd);

            obj.pivot = new Vector2(0.5f, 0.5f);
            obj.sizeDelta = new Vector2(Mathf.Abs(bottomRight.x - topLeft.x), Mathf.Abs(bottomRight.y - topLeft.y));
            obj.localScale = Vector3.one;

            obj.anchorMin = new Vector2(0.5f, 0.5f);
            obj.anchorMax = new Vector2(0.5f, 0.5f);

            Vector3 sizeDelta = obj.sizeDelta / 2f;
            sizeDelta.y *= -1;

            obj.transform.localPosition = topLeft + sizeDelta;
            obj.SetParent(text.transform.parent);
            obj.SetSiblingIndex(text.transform.GetSiblingIndex() + 1);
        }
    }
}
