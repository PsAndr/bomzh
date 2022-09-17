using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Engine
{
    [AddComponentMenu("Engine/Text Help Box/Text Help Box Prefabs Controller")]
    public class TextHelpBoxPrefabsController : MonoBehaviour
    {
        [SerializeField] private Element[] prefabs = new Element[0];

        [System.Serializable]
        private class Element
        {
            [SerializeField] public string name;
            [SerializeField] public GameObject prefab;
        }

        public GameObject Get(string name)
        {
            foreach (Element el in prefabs)
            {
                if (el.name == name)
                {
                    return el.prefab;
                }
            }

            return null;
        }

        public GameObject Get(int number)
        {
            if (number < 0 || number >= prefabs.Length)
            {
                return null;
            }

            return prefabs[number].prefab;
        }

        public int GetIndexFromName(string name)
        {
            for (int index = 0; index < prefabs.Length; index++)
            {
                if (prefabs[index].name == name)
                {
                    return index;
                }
            }

            return -1;
        }

        public int Count()
        {
            return prefabs.Length;
        }

        public void CheckPrefabs()
        {
            foreach (Element el in prefabs)
            {
                if (el.prefab.GetComponent<TextHelpBox>() == null)
                {
                    el.prefab.AddComponent<TextHelpBox>();
                }

                if (el.prefab.GetComponent<RectTransform>() == null)
                {
                    el.prefab.AddComponent<RectTransform>();
                }
            }
        }

        public void CheckNames()
        {
            SortedSet<string> names = new();

            foreach (Element el in prefabs)
            {
                int index = 0;
                string name = el.name;

                while (names.Contains(el.name))
                {
                    el.name = name + $"({index})";
                    index++;
                }

                names.Add(el.name);
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(TextHelpBoxPrefabsController))]
    public class TextHelpBoxPrefabsControllerEditor : Editor
    {
        private TextHelpBoxPrefabsController textHelpBoxPrefabsController;

        private void OnEnable()
        {
            textHelpBoxPrefabsController = (TextHelpBoxPrefabsController)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Check();

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        private void Check()
        {
            textHelpBoxPrefabsController.CheckNames();
            textHelpBoxPrefabsController.CheckPrefabs();
        }
    }
#endif
}
