using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using System;

public class TextPrintingClass : MonoBehaviour
{
    private TextMeshProUGUI text_print;
    private string text_to_print;
    private Global_control global_Control;
    private float letters_to_print;

    private Coroutine coroutine;

    public void Init(Global_control global_Control, TextMeshProUGUI text_print, string text_to_print)
    {
        this.global_Control = global_Control;
        this.text_print = text_print;
        this.text_to_print = text_to_print;
        this.letters_to_print = 0f;
        this.text_print.text = "";

        StartPrinting();
    }

    private void StartPrinting()
    {
        this.coroutine = StartCoroutine("PrintingText");
    }
    private void StopPrinting()
    {
        StopCoroutine(this.coroutine);
    }

    IEnumerator PrintingText()
    {
        float speed = this.global_Control.speed_printing_text;

        int i = 0;

        while (i < this.text_to_print.Length)
        {
            this.letters_to_print += speed / (1f / 0.02f);

            while (letters_to_print >= 1f && i < this.text_to_print.Length)
            {
                this.text_print.text += this.text_to_print[i];

                this.letters_to_print -= 1f;
                i++;
            }
            yield return new WaitForSeconds(0.02f);
        }

        StopPrinting();
    }
}
