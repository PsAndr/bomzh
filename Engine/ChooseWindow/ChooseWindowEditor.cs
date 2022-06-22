#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

namespace Engine
{
    [CustomEditor(typeof(ChooseWindow))]
    public class ChooseWindowEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();

            serializedObject.ApplyModifiedProperties();

            ChooseWindow chooseWindow = (ChooseWindow)target;

            if (chooseWindow.GetItem() != null && chooseWindow.GetItem().GetComponentInChildren<TextMeshProUGUI>() == null)
            {
                Debug.LogError(new System.Exception("Wrong item to choose window!\nDon`t can get component of text"));
            }
            else if (chooseWindow.GetItem() != null && chooseWindow.GetItem().GetComponentInChildren<Button>() == null)
            {
                Debug.LogError(new System.Exception("Wrong item to choose window!\nDon`t can get component of button"));
            }
            else if(chooseWindow.GetItem() != null && chooseWindow.GetItem().GetComponent<ChooseWindowItem>() == null)
            {
                chooseWindow.GetItem().AddComponent<ChooseWindowItem>();
            }
        }
    }
}
#endif
