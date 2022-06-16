using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Engine
{
    public class WaitSceneCommand : MonoBehaviour
    {
        public void StartWait(Global_control global_Control, float time)
        {
            StartCoroutine(this.Wait(global_Control, time));
        }

        IEnumerator Wait(Global_control global_Control, float time)
        {
            global_Control.handlerCommandScene.isWaiting = true;
            yield return new WaitForSeconds(time);
            global_Control.handlerCommandScene.isWaiting = false;
            global_Control.NewCommandScene();
            yield break;
        }
    }
}
