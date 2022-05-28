using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    [SerializeField] private AnimationClip animationClip_open;
    [SerializeField] private AnimationClip animationClip_close;

    public void OpenWindow()
    {
        gameObject.SetActive(true);
        if (animationClip_open != null)
        {
            GetComponent<Animator>().Play(animationClip_open.name);
        }
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
        if (animationClip_close != null)
        {
            GetComponent<Animator>().Play(animationClip_close.name);
        }
    }
}
