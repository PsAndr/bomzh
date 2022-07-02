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
            public Vector2 pivot;
            public Vector2 anchorMax;
            public Vector2 anchorMin;

            public RectTransformSaveValues(RectTransform rectTransform)
            {
                this.localPosition = rectTransform.localPosition;
                this.localScale = rectTransform.localScale;
                this.localRotation = rectTransform.localRotation;
                this.sizeDelta = rectTransform.sizeDelta;
                this.pivot = rectTransform.pivot;
                this.anchorMax = rectTransform.anchorMax;
                this.anchorMin = rectTransform.anchorMin;
            }

            public void UpdateRectTransform(RectTransform rectTransform)
            {
                rectTransform.localPosition = this.localPosition;
                rectTransform.localScale = this.localScale;
                rectTransform.localRotation = this.localRotation;
                rectTransform.sizeDelta = this.sizeDelta;
                rectTransform.pivot = this.pivot;
                rectTransform.anchorMax = this.anchorMax;
                rectTransform.anchorMin = this.anchorMin;
            }
        }

        [System.Serializable]
        public class RectTransformSaveValuesSerializable
        {
            public float[] localPosition;
            public float[] localScale;
            public float[] localRotation;
            public float[] sizeDelta;
            public float[] pivot;
            public float[] anchorMax;
            public float[] anchorMin;

            public RectTransformSaveValuesSerializable(float[] localPosition, float[] localScale, float[] localRotation, float[] sizeDelta, float[] pivot, float[] anchorMax, float[] anchorMin)
            {
                this.localPosition = localPosition;
                this.localScale = localScale;
                this.localRotation = localRotation;
                this.sizeDelta = sizeDelta;
                this.pivot = pivot;
                this.anchorMax = anchorMax;
                this.anchorMin = anchorMin;
            }

            public RectTransformSaveValuesSerializable(RectTransform rectTransform)
            {
                this.localPosition = WorkWithVectors.ConvertToArrayVector(rectTransform.localPosition);
                this.localScale = WorkWithVectors.ConvertToArrayVector(rectTransform.localScale);
                this.localRotation = WorkWithVectors.ConvertToArrayVector(rectTransform.localRotation.eulerAngles);
                this.sizeDelta = WorkWithVectors.ConvertToArrayVector(rectTransform.sizeDelta);
                this.pivot = WorkWithVectors.ConvertToArrayVector(rectTransform.pivot);
                this.anchorMax = WorkWithVectors.ConvertToArrayVector(rectTransform.anchorMax);
                this.anchorMin = WorkWithVectors.ConvertToArrayVector(rectTransform.anchorMin);
            }

            public RectTransformSaveValuesSerializable(Vector3 localPosition, Vector3 localScale, Quaternion localRotationQ, Vector2 sizeDelta, Vector2 pivot)
            {
                Vector3 localRotation = localRotationQ.eulerAngles;
                this.localPosition = WorkWithVectors.ConvertToArrayVector(localPosition);
                this.localScale = WorkWithVectors.ConvertToArrayVector(localScale);
                this.localRotation = WorkWithVectors.ConvertToArrayVector(localRotation);
                this.sizeDelta = WorkWithVectors.ConvertToArrayVector(sizeDelta);
                this.pivot = WorkWithVectors.ConvertToArrayVector(pivot);
                this.anchorMax = WorkWithVectors.ConvertToArrayVector(new Vector2(0.5f, 0.5f));
                this.anchorMin = WorkWithVectors.ConvertToArrayVector(new Vector2(0.5f, 0.5f));

            }

            public RectTransformSaveValuesSerializable(Vector3 localPosition, Vector3 localScale, Vector3 localRotation, Vector2 sizeDelta, Vector2 pivot)
            {
                this.localPosition = WorkWithVectors.ConvertToArrayVector(localPosition);
                this.localScale = WorkWithVectors.ConvertToArrayVector(localScale);
                this.localRotation = WorkWithVectors.ConvertToArrayVector(localRotation);
                this.sizeDelta = WorkWithVectors.ConvertToArrayVector(sizeDelta);
                this.pivot = WorkWithVectors.ConvertToArrayVector(pivot);
                this.anchorMax = WorkWithVectors.ConvertToArrayVector(new Vector2(0.5f, 0.5f));
                this.anchorMin = WorkWithVectors.ConvertToArrayVector(new Vector2(0.5f, 0.5f));
            }

            public RectTransformSaveValuesSerializable(RectTransformSaveValues rectTransformSaveValues)
            {
                this.localPosition = WorkWithVectors.ConvertToArrayVector(rectTransformSaveValues.localPosition);
                this.localScale = WorkWithVectors.ConvertToArrayVector(rectTransformSaveValues.localScale);
                this.localRotation = WorkWithVectors.ConvertToArrayVector(rectTransformSaveValues.localRotation.eulerAngles);
                this.sizeDelta = WorkWithVectors.ConvertToArrayVector(rectTransformSaveValues.sizeDelta);
                this.pivot = WorkWithVectors.ConvertToArrayVector(rectTransformSaveValues.pivot);
                this.anchorMax = WorkWithVectors.ConvertToArrayVector(rectTransformSaveValues.anchorMax);
                this.anchorMin = WorkWithVectors.ConvertToArrayVector(rectTransformSaveValues.anchorMin);
            }

            public void UpdateRectTransform(RectTransform rectTransform)
            {
                rectTransform.localPosition = WorkWithVectors.ConvertArrayToVector3(this.localPosition);
                rectTransform.localScale = WorkWithVectors.ConvertArrayToVector3(this.localScale);
                rectTransform.localRotation = Quaternion.Euler(WorkWithVectors.ConvertArrayToVector3(this.localRotation));
                rectTransform.sizeDelta = WorkWithVectors.ConvertArrayToVector2(this.sizeDelta);
                rectTransform.pivot = WorkWithVectors.ConvertArrayToVector2(this.pivot);
                rectTransform.anchorMin = WorkWithVectors.ConvertArrayToVector2(this.anchorMin);
                rectTransform.anchorMax = WorkWithVectors.ConvertArrayToVector2(this.anchorMax);
            }

            public static implicit operator RectTransformSaveValuesSerializable(RectTransform rectTransform)
            {
                return new RectTransformSaveValuesSerializable(rectTransform);
            }
        }
    }
}
