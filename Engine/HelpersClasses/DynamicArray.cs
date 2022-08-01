using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Engine
{
    [System.Serializable]
    public class DynamicArray<T> : IEnumerable<T>
    {
        public int Length { get; private set; }
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

        public void Change(int index, T value)
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

            values[index] = value;
        }

        public void Add(T value)
        {
            Resize(this.Length + 1);
            values[this.Length - 1] = value;
        }

        public void AddFront(T value)
        {
            Resize(this.Length + 1);
            CircularShift();
            values[0] = value;
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

        public DynamicArray()
        {
            this.values = new T[0];
            Length = 0;
        }

        public DynamicArray(T value)
        {
            this.values = new T[] { value };
            Length = 1;
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

        public void Sort()
        {
            List<T> list = new List<T>(values);
            list.Sort();
            values = list.ToArray();
        }

        public void Sort(IComparer<T> comparer)
        {
            List<T> list = new List<T>(values);
            list.Sort(comparer);
            values = list.ToArray();
        }

        public void CircularShift()
        {
            T valueNow = Get(-1);
            for (int i = 0; i < this.Length; i++)
            {
                SwapClass.Swap(ref valueNow, ref this.values[i]);
            }
        }

        public void CircularShift(int count)
        {
            for (int i = 0; i < count; i++)
            {
                CircularShift();
            }
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

        public T this[int i]
        {
            get { return this.Get(i); }
            set { Change(i, value); }
        }

        public T[] ToArray()
        {
            return this.values;
        }

        public static implicit operator DynamicArray<T>(T[] values)
        {
            return new DynamicArray<T>(values);
        }

        public override string ToString()
        {
            string s = "[";

            foreach (T value in this.values)
            {
                s += value.ToString() + ", ";
            }

            if (s.Length > 1)
            {
                s = s[..^2];
            }

            s += "]";

            return s;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new DynamicArrayEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class DynamicArrayEnumerator<T2> : IEnumerator<T2>
        {
            private DynamicArray<T2> array;
            private int index;
            private T2 value;

            public DynamicArrayEnumerator(DynamicArray<T2> item)
            {
                array = item;
                index = -1;
                value = default(T2);
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public T2 Current
            {
                get { return value; }
            }

            public bool MoveNext()
            {
                index++;
                if (index >= array.Size())
                {
                    return false;
                }
                else
                {
                    value = array[index];
                }
                return true;
            }

            public void Reset()
            {
                this.index = -1;
            }

            public void Dispose()
            {

            }
        }
    }
}
