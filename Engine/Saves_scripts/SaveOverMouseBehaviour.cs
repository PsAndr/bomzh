using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Engine
{
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

        /*public void OnGUI()
        {
            Debug.Log(Event.current.mousePosition);
        }*/

        public void OnDisable()
        {
            parentScript.Exit();
        }
    }
}
