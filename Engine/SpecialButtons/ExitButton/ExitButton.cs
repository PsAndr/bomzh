using UnityEngine.UI;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Engine
{
    [AddComponentMenu("Engine/Special Buttons/Exit Button")]
    public class ExitButton : MonoBehaviour
    {
        [SerializeField] private Button exitButton;

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            if (this.exitButton == null)
            {
                this.exitButton = GetComponent<Button>();
            }

            if (this.exitButton == null)
            {
                this.exitButton = GetComponentInChildren<Button>();
            }

            if (this.exitButton == null)
            {
                this.exitButton = GetComponentInParent<Button>();
            }

            if (this.exitButton != null)
            {
                try
                {
                    this.exitButton.onClick.RemoveListener(this.OnClick);
                }
                catch { }

                this.exitButton.onClick.AddListener(this.OnClick);
            }
        }

        private void OnClick()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ExitButton))]
    public class ExitButtonEditor : Editor
    {
        ExitButton exitButton;

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
            this.exitButton = (ExitButton)target;
            this.exitButton.Init();
        }
    }
#endif
}
