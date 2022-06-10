using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
    public class TextMeshProUGUISclaeWithText
    {
        public static void Scale(TextMeshProUGUI text)
        {
            float height = text.preferredHeight;
            float width = text.gameObject.GetComponent<RectTransform>().sizeDelta.x;

            text.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        }
    }
}
