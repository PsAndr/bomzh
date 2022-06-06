using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
[AddComponentMenu("Engine/Load/Module")]
public class LoadModule : MonoBehaviour
{
    [SerializeField] private Global_control global_Control;

    [SerializeField] private TextMeshProUGUI nameLoad;

    [SerializeField] private Image screenshotImage;

    [SerializeField] private Button loadButton;

    [SerializeField] private LoadOverMouse[] overMouse;

    [HideInInspector] public LoadWindow loadWindow;

    public void Init(string nameLoad)
    {
        if (this.global_Control == null)
        {
            this.global_Control = FindObjectOfType<Global_control>();
        }
        this.nameLoad.text = nameLoad;

        this.loadButton.onClick.AddListener(this.OnClick);

        foreach (LoadOverMouse loadOverMouse in overMouse)
        {
            loadOverMouse.Init();
        }

        this.screenshotImage.sprite = this.global_Control.screenshotSaverLoader.GetScreeenshot(nameLoad);
    }

    private void OnClick()
    {
        loadWindow.Load(this.nameLoad.text);
    }
}
