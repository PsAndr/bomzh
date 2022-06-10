using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using System;

namespace Engine
{
    public class TextPrintingClass : MonoBehaviour
    {
        private TextMeshProUGUI text_print;
        private string text_to_print;
        private Global_control global_Control;
        private float letters_to_print;

        private bool IsPause = false;

        private int indexPrint = 0;

        public void Init(Global_control global_Control, TextMeshProUGUI text_print, string text_to_print, int indexStart = 0)
        {
            StopAllCoroutines();

            this.global_Control = global_Control;
            this.text_print = text_print;
            this.text_to_print = text_to_print;
            this.letters_to_print = 0f;
            this.text_print.text = text_to_print;

            this.IsPause = false;

            float width = this.text_print.transform.GetComponent<RectTransform>().sizeDelta.x;
            this.text_print.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(width, this.text_print.preferredHeight);

            this.text_print.text = text_to_print[..indexStart];

            StartPrinting(indexStart);
        }

        private void StartPrinting(int indexStart)
        {
            global_Control.handlerCommandScene.IsPrintingText = true;
            StartCoroutine(this.PrintingText(indexStart));
        }

        public void StopPrinting()
        {
            this.text_print.text = this.text_to_print;

            global_Control.handlerCommandScene.IsPrintingText = false;

            this.indexPrint = 0;

            StopAllCoroutines();
        }

        public void PausePrinting()
        {
            this.IsPause = true;
        }

        public void ResumePrinting()
        {
            this.IsPause = false;
        }

        IEnumerator PrintingText(int indexStart)
        {
            float speed = this.global_Control.speed_printing_text;

            int i = indexStart;
            this.indexPrint = i;

            while (i < this.text_to_print.Length)
            {
                while (this.IsPause)
                {
                    yield return null;
                }

                this.letters_to_print += speed / (1f / 0.02f);

                while (this.letters_to_print >= 1f && i < this.text_to_print.Length)
                {
                    this.text_print.text += this.text_to_print[i];

                    this.letters_to_print -= 1f;
                    i++;
                    this.indexPrint = i;
                }

                while (this.IsPause)
                {
                    yield return null;
                }
                yield return new WaitForSeconds(0.02f);
            }
            StopPrinting();

            yield break;
        }

        public int GetProgress()
        {
            return this.indexPrint;
        }
    }
}
