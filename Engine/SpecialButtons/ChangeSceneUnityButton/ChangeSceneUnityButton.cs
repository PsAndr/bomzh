using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Engine
{
    [AddComponentMenu("Engine/Special Buttons/Change Scene Unity Button")]
    public class ChangeSceneUnityButton : MonoBehaviour
    {
        [SerializeField] private Button changeButton;

        [SerializeField] private ChooserSceneUnity chooserSceneUnity;

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            if (changeButton == null)
            {
                changeButton = GetComponent<Button>();
            }
            
            if (changeButton == null)
            {
                changeButton = GetComponentInChildren<Button>();
            }

            if (changeButton == null)
            {
                changeButton = GetComponentInParent<Button>();
            }

            if (changeButton != null)
            {
                try
                {
                    changeButton.onClick.RemoveListener(this.OnClick);
                }
                catch { }

                changeButton.onClick.AddListener(this.OnClick);
            }
        }

        private void OnClick()
        {
            FindObjectOfType<SceneUnityController>().LoadNewScene(chooserSceneUnity.NameScene);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ChangeSceneUnityButton))]
    public class ChangeSceneUnityButtonEditor : Editor
    {
        ChangeSceneUnityButton changeSceneUnityButton;

        private void Awake()
        {
            CheckButton();
        }

        private void OnEnable()
        {
            CheckButton();
        }

        private void OnDisable()
        {
            CheckButton();
        }

        private void CheckButton()
        {
            changeSceneUnityButton = (ChangeSceneUnityButton)target;
            try
            {
                changeSceneUnityButton.Init();
            }
            catch { }
        }
    }
#endif
}
