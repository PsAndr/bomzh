using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine.WorkWithRectTransform;
using WorkWithDictionary;

namespace Engine
{
    [CreateAssetMenu(fileName = "SceneNow", menuName = "Engine/SceneNow")]
    [System.Serializable]
    public class SceneNow : ScriptableObject
    {
        [SerializeField] private SceneEngine sceneEngine = new SceneEngine();

        [SerializeField] public string backgroundName;
        [SerializeField] public AudioHelper.SaveClass[] audioHelpers;
        [SerializeField] public VideoHelper.SaveClass[] videoHelpers;   

        [SerializeField] public DictionaryToTwoArrays<string, int> flags;
        [SerializeField] public int indexPrint;

        [SerializeField] public string textOnSceneDialogue;
        [SerializeField] public string textOnSceneCharacter;

        [SerializeField] public string[] spritesNames;
        [SerializeField] public string[] spritesObjectNames;
        [SerializeField] public RectTransformSaveValuesSerializable[] rectTransformsSprites;
        [SerializeField] public SettingsGlobalControl settingsGlobalControl;

        public void SetDefault()
        {
            sceneEngine.sceneNumber = -1;
            sceneEngine.sceneName = null;
            sceneEngine.numberCommandScene = 0;
            backgroundName = null;
            audioHelpers = null;
            videoHelpers = null;
            flags = null;
            indexPrint = 0;
            textOnSceneDialogue = null;
            textOnSceneCharacter = null;
            spritesNames = null;
            spritesObjectNames = null;
            rectTransformsSprites = null;
            settingsGlobalControl = new SettingsGlobalControl();
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

        public void SetValues(Save_class saveClass)
        {
            this.sceneEngine.sceneNumber = saveClass.scene_number;
            this.sceneEngine.sceneName = null;
            this.sceneEngine.numberCommandScene = saveClass.number_command_scene;
            this.backgroundName = saveClass.nameBackground;
            this.audioHelpers = saveClass.audioHelpers;
            this.flags = saveClass.flags;
            this.indexPrint = saveClass.indexPrint;
            this.textOnSceneDialogue = saveClass.textOnSceneDialogue;
            this.textOnSceneCharacter = saveClass.textOnSceneCharacter;
            this.spritesNames = saveClass.spritesNames;
            this.spritesObjectNames = saveClass.spritesObjectNames;
            this.rectTransformsSprites = saveClass.rectTransformsSprites;
            this.settingsGlobalControl = saveClass.settingsGlobalControl;
            this.videoHelpers = saveClass.videoHelpers;
        }

        public SceneEngine GetValue()
        {
            return this.sceneEngine;
        }

        public Save_class GetAllValues()
        {
            Save_class save_Class = new()
            {
                flags = this.flags,
                audioHelpers = this.audioHelpers,
                nameBackground = this.backgroundName,
                number_command_scene = this.sceneEngine.numberCommandScene,
                scene_number = this.sceneEngine.sceneNumber,
                indexPrint = this.indexPrint,
                textOnSceneCharacter = this.textOnSceneCharacter,
                textOnSceneDialogue = this.textOnSceneDialogue,
                spritesNames = this.spritesNames,
                spritesObjectNames = this.spritesObjectNames,
                rectTransformsSprites = this.rectTransformsSprites,
                settingsGlobalControl = this.settingsGlobalControl.Clone() as SettingsGlobalControl,
                videoHelpers = this.videoHelpers,
            };

            return save_Class;
        }
    }
}
