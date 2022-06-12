using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using Engine.WorkWithRectTransform;

namespace Engine
{
    [Serializable]
    [AddComponentMenu("Engine/Save/Module")]
    public class SaveModule : MonoBehaviour
    {
        [SerializeField] private Global_control global_Control;

        [SerializeField] private TMP_InputField name_save;

        [SerializeField] private SaveOverMouse[] overMouse;

        [SerializeField] private Button button;
        [SerializeField] private Button startChangeName;
        [SerializeField] private Button deleteButton;

        [SerializeField] private Image screenshotImage;

        [HideInInspector] public bool IsNew;

        [HideInInspector] public int numberSave;

        [HideInInspector] public string nameSave = "Имя сохранения";

        [HideInInspector] private readonly char[] forbiddenSymbolsPath = { '/', '\\', '*', '\"', '?', '<', '>', '|' };

        [HideInInspector] public SaveWindow saveWindow;

        public void Init()
        {
            if (this.global_Control == null)
            {
                this.global_Control = FindObjectOfType<Global_control>();
            }
            this.name_save.text = this.nameSave;

            this.button.onClick.AddListener(OnClick);

            this.name_save.onValueChanged.AddListener(this.NameChange);
            this.name_save.onDeselect.AddListener(this.DeselectName);
            this.name_save.onEndEdit.AddListener(this.EndChangeName);

            this.deleteButton.onClick.AddListener(Delete);

            if (this.startChangeName == null)
            {
                this.name_save.readOnly = false;
            }
            else
            {
                this.startChangeName.onClick.AddListener(CanChangeName);
                this.name_save.readOnly = true;
            }

            int i = 0;
            foreach (SaveOverMouse saveOverMouse in this.overMouse)
            {
                saveOverMouse.Init(i, this);
                i++;
            }

            if (!this.IsNew)
            {
                this.screenshotImage.sprite = global_Control.screenshotSaverLoader.GetScreeenshot(this.nameSave);
            }
            else
            {
                this.screenshotImage.enabled = false;
            }
        }

        void Start()
        {

        }

        /*private void OnGUI()
        {
            //Debug.Log(Event.current.mousePosition.ToString());
        }*/

        void Update()
        {

        }

        public void Enter(int num)
        {
            if (this.IsNew)
            {
                if (this.overMouse[num].coveringMouseImage_new == null)
                {
                    return;
                }
                this.overMouse[num].coveringMouseImage_new.SetActive(true);
            }
            else
            {
                if (this.overMouse[num].coveringMouseImage_change == null)
                {
                    return;
                }
                this.overMouse[num].coveringMouseImage_change.SetActive(true);
            }
        }

        public void Exit(int num)
        {
            if (this.IsNew)
            {
                if (this.overMouse[num].coveringMouseImage_new == null)
                {
                    return;
                }
                this.overMouse[num].coveringMouseImage_new.SetActive(false);
            }
            else
            {
                if (this.overMouse[num].coveringMouseImage_change == null)
                {
                    return;
                }
                this.overMouse[num].coveringMouseImage_change.SetActive(false);
            }
        }

        private void OnClick()
        {
            if (this.IsNew)
            {
                AudioHelper[] audioHelpers = this.global_Control.audioHandler.gameObject.GetComponents<AudioHelper>();
                AudioHelper.SaveClass[] saveClasses = new AudioHelper.SaveClass[audioHelpers.Length];

                for (int i = 0; i < audioHelpers.Length; i++)
                {
                    saveClasses[i] = audioHelpers[i].GetSave();
                }

                Image[] sprites = global_Control.ToSpawnSprite.GetComponentsInChildren<Image>();
                string[] spritesNames = new string[sprites.Length]; 
                string[] spritesNamesObjects = new string[sprites.Length]; 
                RectTransformSaveValuesSerializable[] rectTransformsSprites = new RectTransformSaveValuesSerializable[sprites.Length]; 

                for (int i = 0; i < sprites.Length; i++)
                {
                    spritesNames[i] = sprites[i].sprite.name;
                    spritesNamesObjects[i] = sprites[i].gameObject.name;
                    rectTransformsSprites[i] = new RectTransformSaveValuesSerializable(sprites[i].gameObject.GetComponent<RectTransform>());
                }

                TextPrintingClass textPrintingClass = global_Control.gameObject.GetComponent<TextPrintingClass>();

                Save_class newSave = new Save_class(this.global_Control.GetSceneValues().first, this.global_Control.GetSceneValues().second, 
                    this.global_Control.Flags, saveClasses, this.global_Control.background.sprite.name, 
                    textPrintingClass.GetProgress(), global_Control.text_dialogue.text, global_Control.text_character.text, 
                    spritesNames, spritesNamesObjects, rectTransformsSprites);

                this.nameSave = newSave.name_save;
                this.name_save.text = newSave.name_save;

                this.IsNew = false;

                foreach (SaveOverMouse saveOverMouse in this.overMouse)
                {
                    if (saveOverMouse.coveringMouseImage_change != null)
                    {
                        if (saveOverMouse.IsActive)
                        {
                            if (saveOverMouse.coveringMouseImage_new != null)
                            {
                                saveOverMouse.coveringMouseImage_new.SetActive(false);
                            }
                            saveOverMouse.coveringMouseImage_change.SetActive(true);
                        }
                    }
                }

                this.saveWindow.SpawnNewSave();
            }
            else
            {
                AudioHelper[] audioHelpers = this.global_Control.audioHandler.gameObject.GetComponents<AudioHelper>();
                AudioHelper.SaveClass[] saveClasses = new AudioHelper.SaveClass[audioHelpers.Length];

                for (int i = 0; i < audioHelpers.Length; i++)
                {
                    saveClasses[i] = audioHelpers[i].GetSave();
                }

                Image[] sprites = global_Control.ToSpawnSprite.GetComponentsInChildren<Image>();
                string[] spritesNames = new string[sprites.Length];
                string[] spritesNamesObjects = new string[sprites.Length];
                RectTransformSaveValuesSerializable[] rectTransformsSprites = new RectTransformSaveValuesSerializable[sprites.Length];

                for (int i = 0; i < sprites.Length; i++)
                {
                    spritesNames[i] = sprites[i].sprite.name;
                    spritesNamesObjects[i] = sprites[i].gameObject.name;
                    rectTransformsSprites[i] = new RectTransformSaveValuesSerializable(sprites[i].gameObject.GetComponent<RectTransform>());
                }

                TextPrintingClass textPrintingClass = global_Control.gameObject.GetComponent<TextPrintingClass>();

                new Save_class(this.nameSave).Change(this.global_Control.GetSceneValues().first, this.global_Control.GetSceneValues().second,
                    this.global_Control.Flags, saveClasses, this.global_Control.background.sprite.name,
                    textPrintingClass.GetProgress(), global_Control.text_dialogue.text, global_Control.text_character.text, 
                    spritesNames, spritesNamesObjects, rectTransformsSprites);

                Debug.Log(Application.persistentDataPath);
            }
            global_Control.screenshotSaverLoader.MakeScreenshot(this.nameSave, this.global_Control);
            StartCoroutine(this.WaitScreenshot());
        }

        private void Delete()
        {
            global_Control.screenshotSaverLoader.Delete(this.nameSave);
            new Save_class(this.nameSave).Delete();
            this.saveWindow.StartWaitDeleteModule();
            Destroy(this.gameObject);
        }

        private void CanChangeName()
        {
            this.name_save.readOnly = false;
            EventSystem.current.SetSelectedGameObject(name_save.gameObject);
        }

        private void NameChange(string s)
        {

        }

        private void DeselectName(string s)
        {

        }

        private void EndChangeName(string s)
        {
            foreach (char ch in this.forbiddenSymbolsPath)
            {
                s = s.Replace(ch, '-');
            }

            Save_class save_Class = new Save_class(this.nameSave);
            save_Class.Change(s);

            s = save_Class.name_save;

            this.global_Control.screenshotSaverLoader.ChangeName(this.nameSave, s);

            this.nameSave = s;
            this.name_save.text = s;

            if (this.startChangeName != null)
            {
                this.name_save.readOnly = true;
            }
        }

        IEnumerator WaitScreenshot()
        {
            yield return null;
            yield return null;
            this.screenshotImage.enabled = true;
            this.screenshotImage.sprite = global_Control.screenshotSaverLoader.GetScreeenshot(this.nameSave);
            yield break;
        }
    }
}
