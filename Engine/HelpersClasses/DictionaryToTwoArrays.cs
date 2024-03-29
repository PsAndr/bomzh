using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    namespace WorkWithDictionary
    {
        [System.Serializable]
        public class DictionaryToTwoArrays<TKey, TValue>
        {
            public TKey[] keys;
            public TValue[] values;

            public DictionaryToTwoArrays(Dictionary<TKey, TValue> dictionary)
            {
                this.keys = new TKey[dictionary.Count];
                this.values = new TValue[dictionary.Count];

                int index = 0;
                foreach (KeyValuePair<TKey, TValue> kvp in dictionary)
                {
                    keys[index] = kvp.Key;
                    values[index] = kvp.Value;

                    index++;
                }
            }

            public static implicit operator Dictionary<TKey, TValue>(DictionaryToTwoArrays<TKey, TValue> dictionaryToTwoArrays)
            {
                return dictionaryToTwoArrays.ConvertToDictionary();
            }

            public static implicit operator MyDictionary<TKey, TValue>(DictionaryToTwoArrays<TKey, TValue> dictionaryToTwoArrays)
            {
                return new MyDictionary<TKey, TValue>(dictionaryToTwoArrays);
            }

            public Dictionary<TKey, TValue> ConvertToDictionary()
            {
                Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();

                for (int i = 0; i < this.keys.Length; i++)
                {
                    result.Add(this.keys[i], this.values[i]);
                }

                return result;
            }
        }
    }
}