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

        public class RectTransformSaveValuesSerializable
        {
            public float[] localPosition;
            public float[] localScale;
            public float[] localRotation;
            public float[] sizeDelta;

            public RectTransformSaveValuesSerializable(float[] localPosition, float[] localScale, float[] localRotation, float[] sizeDelta)
            {
                this.localPosition = localPosition;
                this.localScale = localScale;
                this.localRotation = localRotation;
                this.sizeDelta = sizeDelta;
            }

            public RectTransformSaveValuesSerializable(RectTransform rectTransform)
            {
                this.localPosition = WorkWithVectors.ConvertToArrayVector(rectTransform.localPosition);
                this.localScale = WorkWithVectors.ConvertToArrayVector(rectTransform.localScale);
                this.localRotation = WorkWithVectors.ConvertToArrayVector(rectTransform.localRotation.eulerAngles);
                this.sizeDelta = WorkWithVectors.ConvertToArrayVector(rectTransform.sizeDelta);
            }

            public RectTransformSaveValuesSerializable(Vector3 localPosition, Vector3 localScale, Quaternion localRotationQ, Vector2 sizeDelta)
            {
                Vector3 localRotation = localRotationQ.eulerAngles;
                this.localPosition = WorkWithVectors.ConvertToArrayVector(localPosition);
                this.localScale = WorkWithVectors.ConvertToArrayVector(localScale);
                this.localRotation = WorkWithVectors.ConvertToArrayVector(localRotation);
                this.sizeDelta = WorkWithVectors.ConvertToArrayVector(sizeDelta);
            }

            public RectTransformSaveValuesSerializable(Vector3 localPosition, Vector3 localScale, Vector3 localRotation, Vector2 sizeDelta)
            {
                this.localPosition = WorkWithVectors.ConvertToArrayVector(localPosition);
                this.localScale = WorkWithVectors.ConvertToArrayVector(localScale);
                this.localRotation = WorkWithVectors.ConvertToArrayVector(localRotation);
                this.sizeDelta = WorkWithVectors.ConvertToArrayVector(sizeDelta);
            }
            
            public void UpdateRectTransform(RectTransform rectTransform)
            {
                rectTransform.localPosition = WorkWithVectors.ConvertArrayToVector3(this.localPosition);
                rectTransform.localScale = WorkWithVectors.ConvertArrayToVector3(this.localScale);
                rectTransform.localRotation = Quaternion.Euler(WorkWithVectors.ConvertArrayToVector3(this.localRotation));
                rectTransform.sizeDelta = WorkWithVectors.ConvertArrayToVector2(this.sizeDelta);
            }
        }
    }

    [System.Serializable]
    public class DynamicArray<T>
    {
        private int Length;
        private T[] values;

        public T Get(int index)
        {
            if (index < 0)
            {
                index = -index;
                index %= this.Length;
                index = Length - index;
            }

            if (index >= this.Length)
            {
                index %= this.Length;
            }

            return values[index];
        }

        public int Size()
        {
            return this.Length;
        }

        public DynamicArray(T[] values)
        {
            this.values = (T[])values.Clone();
            Length = values.Length;
        }

        public DynamicArray(int Length)
        {
            this.values = new T[Length];
            this.Length = Length;
        }

        public void Resize(int Length)
        {
            T[] newValues = new T[Length];

            for (int i = 0; i < Mathf.Min(Length, this.Length); i++)
            {
                newValues[i] = this.values[i];
            }

            this.values = newValues;
            this.Length = Length;
        }

        public void Reverse()
        {
            T[] newValues = new T[this.Length];

            for (int i = 0; i < this.Length; i++)
            {
                newValues[i] = this.values[this.Length - 1 - i];
            }

            this.values = newValues;
        }
    }
}
