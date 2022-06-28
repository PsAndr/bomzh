using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    namespace WorkWithJSON
    {
        [System.Serializable]
        public class WorkWithJSON_mass<T>
        {
            public T[] item;

            public WorkWithJSON_mass(T[] item)
            {
                this.item = item;
            }

            public static T[] FromJSON(string json)
            {
                var toReturn = JsonUtility.FromJson<T[]>(json);

                return toReturn;

            }

            public string ToJSON()
            {
                return JsonUtility.ToJson(this);
            }

            public string SaveToString()
            {
                return ToJSON();
            }

            public T[] GetItem()
            {
                var toReturn = item;
                return toReturn;
            }
        }
    }
}
