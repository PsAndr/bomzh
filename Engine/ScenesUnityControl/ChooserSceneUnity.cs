using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Engine
{
    [System.Serializable]
    public class ChooserSceneUnity
    {
        [SerializeField, HideInInspector] private string nameScene;
        [SerializeField, HideInInspector] private int indexScene;

        public int IndexScene
        {
            get { return this.indexScene; }
            set { this.indexScene = value; }
        }

        public string NameScene
        {
            get { return this.nameScene; }
            set { this.nameScene = value; }
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ChooserSceneUnity))]
    public class ChooserSceneUnityEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var IndexScene = property.FindPropertyRelative("indexScene");
            var NameScene = property.FindPropertyRelative("nameScene");

            int indexScene = IndexScene.intValue;
            string nameScene = NameScene.stringValue;
            
            GUIContent dropDown = new GUIContent(label);

            var scenesDirty = EditorBuildSettings.scenes;
            string[] scenes = new string[scenesDirty.Length];

            int i = 0;
            foreach (var sceneDirty in scenesDirty)
            {
                string name = sceneDirty.path.Split('/')[^1];
                name = name.Remove(name.IndexOf(".unity"));
                scenes[i] = name;
                i++;
            }

            indexScene = Mathf.Min(indexScene, scenes.Length - 1);
            indexScene = Mathf.Max(0, indexScene);

            position = EditorGUI.PrefixLabel(position, GUIContent.none);

            indexScene = EditorGUI.Popup(position, dropDown.text, indexScene, scenes);

            nameScene = scenes[indexScene];

            IndexScene.intValue = indexScene;
            NameScene.stringValue = nameScene;
        }
    }
#endif
}
