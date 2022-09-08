using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Engine;
using UnityEngine.UI;

namespace Engine.WorkWithTextMeshPro
{
    public static class GetPositionsOfSymbolsInTMPRO
    {
        public static void Get(TextMeshProUGUI text)
        {
            Vector3 topLeft = text.textInfo.characterInfo[0].topLeft;
            Vector3 bottomRight = text.textInfo.characterInfo[0].bottomRight;

            GameObject obj = GameObject.Find("test___image");

            Image image = null;

            if (obj != null)
            {
                image = obj.GetComponent<Image>();
            }

            if (image == null)
            {
                image = new GameObject("test___image").AddComponent<Image>();
            }

            image.transform.localPosition = topLeft;
        }
    }
}
