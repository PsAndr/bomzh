using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Engine
{
    [AddComponentMenu("Engine/Skip Texts/Choose Start And Stop Buttons And Keys")]
    public class ButtonsAndKeysToStartAndStopSkip : MonoBehaviour
    {
        public Global_control global_Control;

        [SerializeField]
        private DynamicArray<Button> buttonsStart = new();

        [SerializeField]
        private DynamicArray<KeyCode> keysStart = new();

        [SerializeField]
        private DynamicArray<Button> buttonsStop = new();

        [SerializeField]
        private DynamicArray<KeyCode> keysStop = new();

        [SerializeField]
        private DynamicArray<Button> buttonsStartAndStop = new();

        [SerializeField]
        private DynamicArray<KeyCode> keysStartAndStop = new();

        private void Awake()
        {
            if (global_Control == null)
            {
                global_Control = FindObjectOfType<Global_control>();
            }

            CheckLists();
        }

        private void Start()
        {
            StartCoroutine(CheckerIsClickedOnKeys());

            foreach (Button button in buttonsStart)
            {
                button.onClick.AddListener(OnButtonStartClick);
            }

            foreach (Button button in buttonsStop)
            {
                button.onClick.AddListener(OnButtonStopClick);
            }

            foreach (Button button in buttonsStartAndStop)
            {
                button.onClick.AddListener(OnButtonStartAndStopClick);
            }
        }

        public void CheckLists()
        {
            List<KeyCode> removeFromStopKeys = new();
            List<KeyCode> removeFromStartKeys = new();
            List<KeyCode> addToStartAndStopKeys = new();

            List<Button> removeFromStopButtons = new();
            List<Button> removeFromStartButtons = new();
            List<Button> addToStartAndStopButtons = new();

            foreach (KeyCode key in this.keysStart)
            {
                if (this.keysStop.Contains(key))
                {
                    removeFromStartKeys.Add(key);
                    removeFromStopKeys.Add(key);
                    addToStartAndStopKeys.Add(key);
                }
                else if (this.keysStartAndStop.Contains(key))
                {
                    removeFromStartKeys.Add(key);
                }
            }

            foreach (KeyCode key in this.keysStop)
            {
                if (this.keysStart.Contains(key))
                {
                    removeFromStartKeys.Add(key);
                    removeFromStopKeys.Add(key);
                    addToStartAndStopKeys.Add(key);
                }
                else if (this.keysStartAndStop.Contains(key))
                {
                    removeFromStopKeys.Add(key);
                }
            }

            foreach (Button button in this.buttonsStart)
            {
                if (this.buttonsStop.Contains(button))
                {
                    removeFromStartButtons.Add(button);
                    removeFromStopButtons.Add(button);
                    addToStartAndStopButtons.Add(button);
                }
                else if (this.buttonsStartAndStop.Contains(button))
                {
                    removeFromStartButtons.Add(button);
                }
            }

            foreach (Button button in this.buttonsStop)
            {
                if (this.buttonsStart.Contains(button))
                {
                    removeFromStartButtons.Add(button);
                    removeFromStopButtons.Add(button);
                    addToStartAndStopButtons.Add(button);
                }
                else if (this.buttonsStartAndStop.Contains(button))
                {
                    removeFromStopButtons.Add(button);
                }
            }

            foreach (KeyCode key in removeFromStartKeys)
            {
                keysStart.Remove(key);
            }

            foreach (KeyCode key in removeFromStopKeys)
            {
                keysStop.Remove(key);
            }

            foreach (KeyCode key in addToStartAndStopKeys)
            {
                if (!keysStartAndStop.Contains(key))
                {
                    keysStartAndStop.Add(key);
                }
            }

            foreach (Button button in removeFromStartButtons)
            {
                buttonsStart.Remove(button);
            }

            foreach (Button button in removeFromStopButtons)
            {
                buttonsStop.Remove(button);
            }

            foreach (Button button in addToStartAndStopButtons)
            {
                if (!buttonsStartAndStop.Contains(button))
                {
                    buttonsStartAndStop.Add(button);
                }
            }
        }

        private void OnButtonStartClick()
        {
            if (global_Control.handlerCommandScene.IsLookScene)
            {
                global_Control.IsSkiping = true;
            }
        }

        private void OnButtonStopClick()
        {
            if (global_Control.handlerCommandScene.IsLookScene)
            {
                global_Control.IsSkiping = false;
            }
        }

        private void OnButtonStartAndStopClick()
        {
            if (global_Control.handlerCommandScene.IsLookScene)
            {
                global_Control.IsSkiping = !global_Control.IsSkiping;
            }
        }

        IEnumerator CheckerIsClickedOnKeys()
        {
            while (true)
            {
                if (global_Control.handlerCommandScene.IsLookScene)
                {
                    bool flagStart = false;
                    foreach (KeyCode keyCode in keysStart) 
                    {
                        if (Input.GetKeyDown(keyCode))
                        {
                            flagStart = true;
                        }
                    }

                    if (flagStart)
                    {
                        global_Control.IsSkiping = true;
                    }

                    bool flagStop = false;
                    foreach (KeyCode keyCode in keysStop)
                    {
                        if (Input.GetKeyDown(keyCode))
                        {
                            flagStop = true;
                        }
                    }

                    if (flagStop)
                    {
                        global_Control.IsSkiping = false;
                    }

                    bool flagStartAndStop = false;
                    foreach (KeyCode keyCode in keysStartAndStop)
                    {
                        if (Input.GetKeyDown(keyCode))
                        {
                            flagStartAndStop = true;
                        }
                    }

                    if (flagStartAndStop)
                    {
                        global_Control.IsSkiping = !global_Control.IsSkiping;
                    }
                }
                yield return null;
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ButtonsAndKeysToStartAndStopSkip))]
    public class ButtonsAndKeysToStartSkipEditor : Editor
    {
        private void Awake()
        {
            CheckGlobalControl();
        }

        private void OnEnable()
        {
            CheckGlobalControl();
            CheckLists();
        }

        private void OnDisable()
        {
            CheckGlobalControl();
            CheckLists();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            CheckGlobalControl();
            CheckLists();

            serializedObject.ApplyModifiedProperties();
        }

        private void CheckGlobalControl()
        {
            ButtonsAndKeysToStartAndStopSkip script = (ButtonsAndKeysToStartAndStopSkip)target;
            if (script.global_Control == null)
            {
                script.global_Control = FindObjectOfType<Global_control>();
            }
        }

        private void CheckLists()
        {
            ButtonsAndKeysToStartAndStopSkip script = (ButtonsAndKeysToStartAndStopSkip)target;
            script.CheckLists();
        }
    }
#endif
}

