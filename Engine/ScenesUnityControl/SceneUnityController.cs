using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Engine
{
    [AddComponentMenu("Engine/ScenesUnity/SceneUnityController")]
    public class SceneUnityController : MonoBehaviour
    {
        private Scene sceneNow;

        private void Start()
        {
            this.sceneNow = gameObject.scene;
        }

        public void LoadNewScene(string name, Slider slider = null)
        {
            StartCoroutine(this.Loading(name, slider));
        }

        public void LoadNewScene(int number, Slider slider = null)
        {
            StartCoroutine(this.Loading(number, slider));
        }

        IEnumerator Loading(string name, Slider slider)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

            while (!asyncLoad.isDone)
            {
                if (slider != null)
                {
                    slider.value = asyncLoad.progress;
                }
                yield return null;
            }
            yield break;
        }

        IEnumerator Loading(int number, Slider slider)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(number);

            while (!asyncLoad.isDone)
            {
                if (slider != null)
                {
                    slider.value = asyncLoad.progress;
                }
                yield return null;
            }
            yield break;
        }
    }
}