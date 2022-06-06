using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Engine/Window/Window")]
[System.Serializable]
public class Window : MonoBehaviour
{
    [SerializeField] private AnimationClip animationClip_open;
    [SerializeField] private AnimationClip animationClip_close;

    private Window previous_window;
    private bool OpenPreviousWindow = false;

    private GameObject[] OpenWindowParts;
    private GameObject[] OpenWindows;

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

        if (this.OpenWindows != null)
        {
            foreach (GameObject obj in this.OpenWindows)
            {
                obj.SetActive(true);
            }
            this.OpenWindows = null;
        }

        return true;
    }

    public bool CloseWindow(GameObject[] OpenWindows = null)
    {
        this.OpenWindows = OpenWindows;

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
