using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LoadOverMouseBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public LoadOverMouse parentScript;

    public void OnPointerEnter(PointerEventData data)
    {
        parentScript.Enter();
    }

    public void OnPointerExit(PointerEventData data)
    {
        parentScript.Exit();
    }

    void OnDisable()
    {
        parentScript.Exit();    
    }
}
