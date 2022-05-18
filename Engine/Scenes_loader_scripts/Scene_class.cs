using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class Scene_class
{
    public int number;
    public string name;

    public Scene_class(int number, string name)
    {
        this.number = number;
        this.name = name;
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }
}
