using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorkWithJSON_mass<T>
{
    public T[] item;

    public WorkWithJSON_mass(T[] item)
    {
        this.item = item;
    }

    public WorkWithJSON_mass(string json)
    {
        this.item = JsonUtility.FromJson<WorkWithJSON_mass<T>>(json).item;
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }
}
