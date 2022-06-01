using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SaveOverMouseBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SaveOverMouse parentScript;

    public void OnPointerEnter(PointerEventData data)
    {
        parentScript.Enter();
    }

    public void OnPointerExit(PointerEventData data)
    {
        parentScript.Exit();
    }
}
