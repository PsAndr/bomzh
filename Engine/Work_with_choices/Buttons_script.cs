using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class Buttons_script : MonoBehaviour
    {
        [SerializeField] public Global_control global_Control;
        public void Is_On_Click()
        {
            global_Control.handlerCommandScene.On_Click_Choices(global_Control, gameObject.name);
        }
    }
}
