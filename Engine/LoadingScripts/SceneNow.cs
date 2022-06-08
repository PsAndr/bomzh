using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

namespace Engine
{
    [CreateAssetMenu(fileName = "SceneNow", menuName = "Engine/SceneNow")]
    [System.Serializable]
    public class SceneNow : ScriptableObject
    {
        [SerializeField] private SceneEngine sceneEngine;

        public void SetDefault()
        {
            sceneEngine.numberCommandScene = -1;
            sceneEngine.sceneName = null;
            sceneEngine.numberCommandScene = 0;
        }

        public void SetValues(int sceneNumber, string sceneName, int numberCommandScene)
        {
            this.sceneEngine.sceneNumber = sceneNumber;
            this.sceneEngine.sceneName = sceneName;
            this.sceneEngine.numberCommandScene = numberCommandScene;
        }

        public void SetValues(SceneNow sceneNow)
        {
            this.sceneEngine.sceneNumber = sceneNow.sceneEngine.sceneNumber + 0;
            this.sceneEngine.sceneName = sceneNow.sceneEngine.sceneName + "";
            this.sceneEngine.numberCommandScene = sceneNow.sceneEngine.numberCommandScene + 0;
        }

        public void SetValues(SceneEngine sceneNow)
        {
            this.sceneEngine.sceneNumber = sceneNow.sceneNumber + 0;
            this.sceneEngine.sceneName = sceneNow.sceneName + "";
            this.sceneEngine.numberCommandScene = sceneNow.numberCommandScene + 0;
        }

        public SceneEngine GetValue()
        {
            return this.sceneEngine;
        }
    }
}
