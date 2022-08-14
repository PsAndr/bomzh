using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine.WorkWithJSON;
using Engine.WorkWithDictionary;
using System;

namespace Engine 
{
    [Serializable]
    public class MyDictionary<TKey, TValue> : ICloneable
    {
        [NonSerialized]
        private const int defaultSize = 8;
        [NonSerialized]
        private const double percentToResize = 0.75;
        [NonSerialized]
        private const double percentToRehash = 0.5;
        [NonSerialized]
        private const int sizeMultiply = 2;

        [SerializeField] private ValueClass[] values;

        [Serializable]
        private class ValueClass
        {
            public TKey Key;
            public TValue Value;

            public bool isDeleted;

            public ValueClass(TKey key, TValue value)
            {
                this.Key = key;
                this.Value = value;
                this.isDeleted = false;
            }
        }

        [SerializeField] private int count;
        [SerializeField] private int countWithDelete; 

        public int Count { get { return count; } }

        public MyDictionary()
        {
            values = new ValueClass[defaultSize];
            count = 0;
        }

        public MyDictionary(Dictionary<TKey, TValue> dictionary)
        {
            values = new ValueClass[defaultSize];
            count = 0;
            countWithDelete = 0;
            foreach (var kvp in dictionary)
            {
                Add(kvp);
            }
        }

        public MyDictionary(MyDictionary<TKey, TValue> dictionary)
        {
            values = new ValueClass[defaultSize];
            count = 0;
            countWithDelete = 0;
            foreach (var kvp in dictionary.GetValues())
            {
                Add(kvp);
            }
        }

        public MyDictionary(DictionaryToTwoArrays<TKey, TValue> dictionaryToTwoArrays)
        {
            values = new ValueClass[defaultSize];
            count = 0;
            countWithDelete = 0;
            for (int i = 0; i < dictionaryToTwoArrays.keys.Length; i++)
            {
                this.Add(dictionaryToTwoArrays.keys[i], dictionaryToTwoArrays.values[i]);
            }
        }

        public MyDictionary(params Pair<TKey, TValue>[] items)
        {
            values = new ValueClass[defaultSize];
            count = 0;
            countWithDelete = 0;
            foreach (var kvp in items)
            {
                Add(kvp);
            }
        }

        public int GetCount()
        {
            return count;
        }

        public static implicit operator MyDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            MyDictionary<TKey, TValue> myDictionary = new(dictionary);

            return myDictionary;
        }

        public static implicit operator Dictionary<TKey, TValue>(MyDictionary<TKey, TValue> myDictionary)
        {
            Dictionary<TKey, TValue> dictionary = new();

            foreach (var kvp in myDictionary.GetValues())
            { 
                dictionary.Add(kvp.first, kvp.second);
            }

            return dictionary;
        }

        public static implicit operator DictionaryToTwoArrays<TKey, TValue>(MyDictionary<TKey, TValue> myDictionary)
        {
            return new DictionaryToTwoArrays<TKey, TValue>(myDictionary);
        }

        private Pair<int, int> GetHashFromHashCode(int hash)
        {
            if (values == null || values.Length == 0)
            {
                values = new ValueClass[defaultSize];
            }

            int toReturn = 0;
            int toReturn2 = 0;

            string hashStr = hash.ToString();

            for (int i = 0; i < hashStr.Length; i++)
            {
                toReturn = (toReturn * (this.values.Length - 1) + hashStr[i]) % this.values.Length;
                toReturn2 = (toReturn * (this.values.Length + 1) + hashStr[i]) % this.values.Length;
            }

            if (toReturn < 0)
            {
                toReturn = -toReturn;
            }
            if (toReturn2 < 0)
            {
                toReturn2 = -toReturn2;
            }

            toReturn = (toReturn * 2 + 1) % this.values.Length;
            toReturn2 = (toReturn2 * 2 + 1) % this.values.Length;

            return (toReturn, toReturn2);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.values.Equals(((MyDictionary<TKey, TValue>)obj).values);
        }

        public void Add(KeyValuePair<TKey, TValue> kvp)
        {
            TryAdd(kvp.Key, kvp.Value);
        }

        public void Add(Pair<TKey, TValue> pair)
        {
            TryAdd(pair.first, pair.second);
        }

        public void Add(TKey key, TValue value)
        {
            TryAdd(key, value);
        }

        public bool TryAdd(TKey key, TValue value)
        {
            if (key == null || value == null)
            {
                return false;
            }
            if (count + 1 > (int)(percentToResize * values.Length))
            {
                Resize();
            }
            else if ((int)(countWithDelete * percentToRehash) > count)
            {
                Rehash();
            }

            int hash = key.GetHashCode();
            Pair<int, int> hashes = GetHashFromHashCode(hash);

            int index = hashes.first % values.Length;

            int firstDeleted = -1;

            int i = 0;

            while (values[index] != null && i < values.Length)
            {
                if (values[index].Key != null && values[index].Key.Equals(key) && !values[index].isDeleted)
                {
                    return false;
                }

                if (values[index].isDeleted && firstDeleted == -1)
                {
                    firstDeleted = index;
                }

                index = (index + hashes.second) % values.Length;
                i++;
            }

            if (firstDeleted == -1)
            {
                values[index] = new ValueClass(key, value);
                countWithDelete++;
            }
            else
            {
                values[firstDeleted].Value = value;
                values[firstDeleted].Key = key;
                values[firstDeleted].isDeleted = false;
            }
            count++;

            return true;
        }

        private void Resize()
        {
            ValueClass[] newValues = new ValueClass[this.values.Length * sizeMultiply];

            count = 0;
            countWithDelete = 0;

            SwapClass.Swap(ref values, ref newValues);

            foreach (ValueClass valueClass in newValues)
            {
                if (valueClass != null && !valueClass.isDeleted)
                {
                    Add(valueClass.Key, valueClass.Value);
                }
            }

            newValues = null;
        }

        private void Rehash()
        {
            ValueClass[] newValues = new ValueClass[this.values.Length];

            count = 0;
            countWithDelete = 0;

            SwapClass.Swap(ref values, ref newValues);

            foreach (ValueClass valueClass in newValues)
            {
                if (valueClass != null && !valueClass.isDeleted)
                {
                    Add(valueClass.Key, valueClass.Value);
                }
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (key == null)
            {
                value = default;
                return false;
            }
            int hash = key.GetHashCode();
            Pair<int, int> hashes = GetHashFromHashCode(hash);

            int index = hashes.first % values.Length;
            int i = 0;

            while (values[index] != null && i < values.Length)
            {
                if (values[index].Key != null && values[index].Key.Equals(key) && !values[index].isDeleted)
                {
                    value = values[index].Value;
                    return true;
                }

                index = (index + hashes.second) % values.Length;
                i++;
            }

            value = default;
            return false;
        }

        public bool TryRemove(TKey key)
        {
            return TryRemove(key, out _);
        }

        public bool TryRemove(TKey key, out TValue value)
        {
            int hash = key.GetHashCode();
            Pair<int, int> hashes = GetHashFromHashCode(hash);

            int index = hashes.first;
            int i = 0;

            while (values[index] != null && i < values.Length)
            {
                if (values[index].Key.Equals(key) && !values[index].isDeleted)
                {
                    values[index].isDeleted = true;
                    value = values[index].Value;
                    count--;
                    return true;
                }

                index = (index + hashes.second) % values.Length;
                i++;
            }
            value = default;
            return false;
        }

        public void Remove(TKey key)
        {
            TryRemove(key);
        }

        public void Remove(TKey key, out TValue value)
        {
            TryRemove(key, out value);
        }

        public bool ContainsKey(TKey key)
        {
            return TryGetValue(key, out _);
        }

        private void ChangeValue(TKey key, TValue value)
        {
            int hash = key.GetHashCode();
            Pair<int, int> hashes = GetHashFromHashCode(hash);

            int index = hashes.first;
            int i = 0;

            while (values[index] != null && i < values.Length)
            {
                if (values[index].Key.Equals(key) && !values[index].isDeleted)
                {
                    values[index].Value = value;
                    return;
                }

                index = (index + hashes.second) % values.Length;
                i++;
            }

            Add(key, value);
        }

        public TValue this[TKey key]
        {
            get
            {
                TryGetValue(key, out TValue toReturn);
                return toReturn;
            }
            set
            {
                ChangeValue(key, value);
            }
        }

        public Pair<TKey, TValue>[] GetValues()
        {
            List<Pair<TKey, TValue>> toReturn = new();
            foreach (ValueClass valueClass in values)
            {
                if (valueClass != null && !valueClass.isDeleted && valueClass.Key != null && valueClass.Value != null)
                {
                    toReturn.Add((valueClass.Key, valueClass.Value));
                }
            }
            return toReturn.ToArray();
        }

        public TValue[] GetOnlyValues()
        {
            List<TValue> toReturn = new();
            foreach (ValueClass valueClass in values)
            {
                if (valueClass != null && !valueClass.isDeleted && valueClass.Key != null && valueClass.Value != null)
                {
                    toReturn.Add(valueClass.Value);
                }
            }
            return toReturn.ToArray();
        }

        public override string ToString()
        {
            return ((DynamicArray<Pair<TKey, TValue>>)GetValues()).ToString();
        }
        public object Clone()
        {
            return new MyDictionary<TKey, TValue>(this);
        }

        public string ToJSON()
        {
            //return new WorkWithJSON_mass<Pair<TKey, TValue>>(GetValues()).ToJSON();
            return JsonUtility.ToJson(this);
        }


        public static MyDictionary<TKey, TValue> FromJSON(string json)
        {
            var result = JsonUtility.FromJson<MyDictionary<TKey, TValue>>(json);
            return result;
        }
    }
}
