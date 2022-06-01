using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveWindow : MonoBehaviour
{
    [SerializeField] GameObject prefabModule;

    [SerializeField] int countLines;
    [SerializeField] int countInLine;

    [SerializeField] Global_control global_Control;

    private void Awake()
    {
        if (this.global_Control == null)
        {
            this.global_Control = FindObjectOfType<Global_control>();
        }
    }
}
