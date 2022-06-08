using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    namespace WorkWithRectTransform
    {
        [System.Serializable]
        public class RectTransformSaveValues
        {
            public Vector3 localPosition;
            public Vector3 localScale;
            public Quaternion localRotation;
            public Vector2 sizeDelta;

            public RectTransformSaveValues(RectTransform rectTransform)
            {
                this.localPosition = rectTransform.localPosition;
                this.localScale = rectTransform.localScale;
                this.localRotation = rectTransform.localRotation;
                this.sizeDelta = rectTransform.sizeDelta;
            }

            public void UpdateRectTransform(RectTransform rectTransform)
            {
                rectTransform.localPosition = this.localPosition;
                rectTransform.localScale = this.localScale;
                rectTransform.localRotation = this.localRotation;
                rectTransform.sizeDelta = this.sizeDelta;
            }
        }
    }
}
