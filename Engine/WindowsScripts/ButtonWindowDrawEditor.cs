using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Engine;

#if UNITY_EDITOR
[CustomEditor(typeof(ButtonsWindow))]
public class ButtonWindowDrawEditor : Editor
{
    SerializedProperty CloseOtherWindows;
    SerializedProperty CloseThisWindow;
    SerializedProperty keyCodes;
    SerializedProperty typeButton;
    SerializedProperty SaveOpenedWindows;

    private void OnEnable()
    {
        SerializedObject property = serializedObject;

        CloseOtherWindows = property.FindProperty("CloseOtherWindows");
        CloseThisWindow = property.FindProperty("CloseThisWindow");
        keyCodes = property.FindProperty("keyCodes");
        typeButton = property.FindProperty("typeButton");
        SaveOpenedWindows = property.FindProperty("SaveOpenedWindows");
    }

    override public void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        serializedObject.Update();

        EditorGUILayout.PropertyField(keyCodes);
        EditorGUILayout.PropertyField(typeButton);

        TypeButtonWindow type = (TypeButtonWindow)typeButton.enumValueIndex;

        switch (type)
        {
            case TypeButtonWindow.Open:
                EditorGUILayout.PropertyField(CloseOtherWindows);
                EditorGUILayout.PropertyField(CloseThisWindow);
                break;

            case TypeButtonWindow.Close:
                EditorGUILayout.PropertyField(SaveOpenedWindows);
                break;

            case TypeButtonWindow.CloseAndOpen:
                EditorGUILayout.PropertyField(CloseOtherWindows);
                EditorGUILayout.PropertyField(CloseThisWindow);
                EditorGUILayout.PropertyField(SaveOpenedWindows);
                break;
        }


        serializedObject.ApplyModifiedProperties();
    }
}
#endif
