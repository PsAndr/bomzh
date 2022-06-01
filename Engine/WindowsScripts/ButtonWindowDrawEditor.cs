using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


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

        TypeButton type = (TypeButton)typeButton.enumValueIndex;

        switch (type)
        {
            case TypeButton.Open:
                EditorGUILayout.PropertyField(CloseOtherWindows);
                EditorGUILayout.PropertyField(CloseThisWindow);
                break;

            case TypeButton.Close:
                EditorGUILayout.PropertyField(SaveOpenedWindows);
                break;

            case TypeButton.CloseAndOpen:
                EditorGUILayout.PropertyField(CloseOtherWindows);
                EditorGUILayout.PropertyField(CloseThisWindow);
                EditorGUILayout.PropertyField(SaveOpenedWindows);
                break;
        }


        serializedObject.ApplyModifiedProperties();
    }
}