using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

public enum TypeButton
{
    Close,
    Open,
    CloseAndOpen
}

[Serializable]
public class ButtonsWindow : MonoBehaviour
{
    private GameObject my_window;
    private Window window_script;

    private string name_open_window;
    private Window openWindow;

    [SerializeField] private TypeButton typeButton;

    [SerializeField] private bool CloseOtherWindows = false;
    [SerializeField] private bool CloseThisWindow = false;

    [SerializeField] private bool SaveOpenedWindows = false;

    [SerializeField] private KeyCode[] keyCodes;

    void Start()
    {
        if (gameObject.name.Split('_').Length < 2)
        {
            Debug.LogException(new Exception("ButtonsWindow script attach to gameobject without: _"));
            return;
        }

        if (gameObject.name.Split('_')[^1] != "button")
        {
            Debug.LogException(new Exception("ButtonsWindow script attach to gameobject without button in name"));
            return;
        }

        if (gameObject.GetComponent<Button>() == null)
        {
            Debug.LogException(new Exception("ButtonsWindow script attach to gameobject without component: Button"));
            return;
        }

        GetComponent<Button>().onClick.AddListener(this.OnClick);

        this.my_window = gameObject;

        while (this.my_window.name.Split('_').Length < 2 || this.my_window.name.Split('_')[1] != "window")
        {
            this.my_window = this.my_window.transform.parent.gameObject;
        }

        this.window_script = this.my_window.GetComponent<Window>();

        switch (this.typeButton)
        {
            case TypeButton.Close:
                break;

            case TypeButton.Open:
                if (gameObject.name.Split('_').Length > 1)
                {
                    this.name_open_window = gameObject.name.Split('_')[0];

                    for (int i = 0; i < this.my_window.transform.childCount; i++)
                    {
                        GameObject game_object = this.my_window.transform.GetChild(i).gameObject;
                        if (game_object.name.Split('_').Length > 1 && game_object.name.Split('_')[0] == this.name_open_window && game_object.name.Split('_')[1] == "window")
                        {
                            this.openWindow = game_object.GetComponent<Window>();
                            break;
                        }
                    }
                }

                break;

            case TypeButton.CloseAndOpen:
                if (gameObject.name.Split('_').Length > 1)
                {
                    this.name_open_window = gameObject.name.Split('_')[0];

                    for (int i = 0; i < this.my_window.transform.childCount; i++)
                    {
                        GameObject game_object = this.my_window.transform.GetChild(i).gameObject;
                        if (game_object.name.Split('_').Length > 1 && game_object.name.Split('_')[0] == this.name_open_window && game_object.name.Split('_')[1] == "window")
                        {
                            this.openWindow = game_object.GetComponent<Window>();
                            break;
                        }
                    }
                }
                break;

            default:
                return;
        }
    }

    public void OnClick()
    {
        List<GameObject> OtherWindowPartsClose = new List<GameObject>();

        switch (this.typeButton)
        {
            case TypeButton.Close:
                for (int i = 0; i < this.my_window.transform.childCount; i++)
                {
                    GameObject game_object = this.my_window.transform.GetChild(i).gameObject;
                    if (game_object.name.Split('_').Length > 1 && game_object.name.Split('_')[1] == "window")
                    {
                        if (game_object.GetComponent<Window>() != null)
                        {
                            if (!this.SaveOpenedWindows)
                            {
                                game_object.GetComponent<Window>().CloseWindow();
                            }
                        }
                    }
                }

                this.window_script.CloseWindow();
                break;

            case TypeButton.Open:
                if (this.openWindow != null && this.openWindow.CanOpenOrClose())
                {
                    if (this.CloseOtherWindows)
                    {
                        for (int i = 0; i < this.my_window.transform.childCount; i++)
                        {
                            GameObject game_object = this.my_window.transform.GetChild(i).gameObject;
                            if (game_object.name.Split('_').Length > 1 && game_object.name.Split('_')[1] == "window" && game_object.name.Split('_')[0] != this.name_open_window)
                            {
                                if (game_object.GetComponent<Window>() != null)
                                {
                                    game_object.GetComponent<Window>().CloseWindow();
                                }
                            }
                        }
                    }

                    if (this.CloseThisWindow)
                    {
                        for (int i = 0; i < this.my_window.transform.childCount; i++)
                        {
                            GameObject game_object = this.my_window.transform.GetChild(i).gameObject;
                            if (game_object.name.Split('_').Length > 1 && game_object.name.Split('_')[1] == "window")
                            {
                                if (game_object.GetComponent<Window>() == null)
                                {
                                    game_object.SetActive(false);
                                    OtherWindowPartsClose.Add(game_object);
                                }
                            }
                            else
                            {
                                game_object.SetActive(false);
                                OtherWindowPartsClose.Add(game_object);
                            }
                        }
                    }

                    this.openWindow.OpenWindow(this.window_script, this.CloseThisWindow, OtherWindowPartsClose.ToArray());
                }
                break;

            case TypeButton.CloseAndOpen:
                if (this.openWindow == null)
                {
                    break;
                }
                
                if (this.openWindow.gameObject.activeSelf)
                {
                    for (int i = 0; i < this.my_window.transform.childCount; i++)
                    {
                        GameObject game_object = this.my_window.transform.GetChild(i).gameObject;
                        if (game_object.name.Split('_').Length > 1 && game_object.name.Split('_')[1] == "window")
                        {
                            if (game_object.GetComponent<Window>() != null)
                            {
                                if (!this.SaveOpenedWindows)
                                {
                                    game_object.GetComponent<Window>().CloseWindow();
                                }
                            }
                        }
                    }

                    this.openWindow.CloseWindow();
                }
                else
                {
                    if (this.CloseOtherWindows)
                    {
                        for (int i = 0; i < this.my_window.transform.childCount; i++)
                        {
                            GameObject game_object = this.my_window.transform.GetChild(i).gameObject;
                            if (game_object.name.Split('_').Length > 1 && game_object.name.Split('_')[1] == "window" && game_object.name.Split('_')[0] != this.name_open_window)
                            {
                                if (game_object.GetComponent<Window>() != null)
                                {
                                    game_object.GetComponent<Window>().CloseWindow();
                                }
                            }
                        }
                    }

                    if (this.CloseThisWindow)
                    {
                        for (int i = 0; i < this.my_window.transform.childCount; i++)
                        {
                            GameObject game_object = this.my_window.transform.GetChild(i).gameObject;
                            if (game_object.name.Split('_').Length > 1 && game_object.name.Split('_')[1] == "window")
                            {
                                if (game_object.GetComponent<Window>() == null)
                                {
                                    game_object.SetActive(false);
                                    OtherWindowPartsClose.Add(game_object);
                                }
                            }
                            else if (!game_object.Equals(gameObject))
                            {
                                game_object.SetActive(false);
                                OtherWindowPartsClose.Add(game_object);
                            }
                        }
                    }

                    this.openWindow.OpenWindow(this.window_script, this.CloseThisWindow, OtherWindowPartsClose.ToArray());
                }
                break;

            default:
                return;
        }
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            CheckKeysClick();
        }
    }

    private void CheckKeysClick()
    {
        foreach (KeyCode keyCode in this.keyCodes)
        {
            if (Input.GetKeyDown(keyCode))
            {
                this.OnClick();
            }
        }
    }
}


