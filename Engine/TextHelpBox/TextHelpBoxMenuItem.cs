#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Engine
{
    public class TextHelpBoxMenuItem : MonoBehaviour
    {
        [MenuItem("GameObject/Engine/Spawn Text Help Box Prefabs Controller", priority = -1, validate = false)]
        private static void Spawn()
        {
            if (FindObjectOfType<TextHelpBoxPrefabsController>() != null)
            {
                SetActiveObject(FindObjectOfType<TextHelpBoxPrefabsController>().gameObject);
                return;
            }

            GameObject textHelpBoxPrefabsController = new GameObject("___TextHelpBoxPrefabsController___");
            textHelpBoxPrefabsController.AddComponent<TextHelpBoxPrefabsController>();

            Global_control globalControl = FindObjectOfType<Global_control>();
            if (globalControl != null)
            {
                int siblingIndex = globalControl.transform.GetSiblingIndex();
                textHelpBoxPrefabsController.transform.SetSiblingIndex(siblingIndex + 1);
                textHelpBoxPrefabsController.transform.SetParent(globalControl.transform.parent);
            }

            SetActiveObject(textHelpBoxPrefabsController);
        }

        private static void SetActiveObject(GameObject obj)
        {
            if (Selection.activeGameObject != obj)
            {
                Selection.activeGameObject = obj;
            }
        }
    }
}
#endif
