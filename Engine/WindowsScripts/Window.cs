using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    [SerializeField] private AnimationClip animationClip_open;
    [SerializeField] private AnimationClip animationClip_close;

    private Window previous_window;
    private bool OpenPreviousWindow = false;

    private GameObject[] OpenWindowParts;

    private bool IsPlayAnimation = false;

    public bool CanOpenOrClose()
    {
        bool flag = true;

        flag = flag && !this.IsPlayAnimation;

        return flag;
    }

    public bool OpenWindow(Window previous_window, bool OpenPreviousWindow, GameObject[] OpenWindowParts)
    {
        if (gameObject.activeSelf || this.IsPlayAnimation)
        {
            return false;
        }

        this.previous_window = previous_window;
        this.OpenPreviousWindow = OpenPreviousWindow;
        this.OpenWindowParts = OpenWindowParts;

        gameObject.SetActive(true);

        if (animationClip_open != null)
        {
            GetComponent<Animator>().Play(animationClip_open.name);
            this.IsPlayAnimation = true;
            StartCoroutine("Open");
        }

        return true;
    }

    public bool CloseWindow()
    {
        if (!gameObject.activeSelf || this.IsPlayAnimation)
        {
            return false;
        }

        if (animationClip_close != null)
        {
            GetComponent<Animator>().Play(animationClip_close.name);
            this.IsPlayAnimation = true;
            StartCoroutine("Close");
        }
        else
        {
            gameObject.SetActive(false);
        }

        if (this.OpenPreviousWindow)
        {
            foreach (GameObject obj in this.OpenWindowParts)
            {
                obj.SetActive(true);
            }
        }

        return true;
    }

    IEnumerator Close()
    {
        yield return new WaitForSeconds(animationClip_close.length);

        gameObject.SetActive(false);

        this.IsPlayAnimation = false;

        StopCoroutine("Close");
    }

    IEnumerator Open()
    {
        yield return new WaitForSeconds(animationClip_open.length);
        this.IsPlayAnimation = false;
        StopCoroutine("Open");
    }
}
