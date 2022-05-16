using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save_attach_gameobject : MonoBehaviour
{
    public void Save(int num_scene, bool[] array_flags, string name_save)
    {
        Save_class to_save = new Save_class(num_scene, array_flags, name_save);

        to_save.save();
    }

    public void Save(int num_scene, bool[] array_flags)
    {
        Save_class to_save = new Save_class(num_scene, array_flags);

        to_save.save();
    }
}
