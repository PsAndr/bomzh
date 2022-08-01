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
            global_Control.handlerCommandScene.isWaiting = true;

            StartCoroutine(this.Wait(global_Control, time));
        }

        public void StopWait(Global_control global_Control)
        {
            StopAllCoroutines();

            if (global_Control.handlerCommandScene.isWaiting)
            {
                global_Control.handlerCommandScene.isWaiting = false;
                global_Control.NewCommandScene();
            }
        }

        IEnumerator Wait(Global_control global_Control, float time)
        {
            while (time > 0f)
            {
                yield return null;
                if (global_Control.handlerCommandScene.IsLookScene)
                {
                    time -= Time.deltaTime;
                }
                if (!global_Control.handlerCommandScene.isWaiting)
                {
                    yield break;
                }
            }

            StopWait(global_Control);
            
            yield break;
        }
    }
}
