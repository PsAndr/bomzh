using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Engine.WorkWithTextMeshPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI text;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        GetPositionsOfSymbolsInTMPRO.Get(text);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Test))]
public class TestEditor : Editor
{
    Test test;
    private void OnEnable()
    {
        test = (Test)target;
        test.Init();
    }
}
#endif
