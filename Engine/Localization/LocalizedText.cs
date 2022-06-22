using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Engine
{
    [AddComponentMenu("Engine/Localization/Localized Text")]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private string nameLocalizedText;
        [SerializeField] private GameObject text;

        private TypeLocalizedText m_TypeLocalizedText;

        public enum TypeLocalizedText
        {
            None,
            Text,
            TextMeshProUGUI
        }

        public GameObject GetText()
        {
            return text;
        }

        public TypeLocalizedText GetTypeLocalizedText()
        {
            return m_TypeLocalizedText;
        }

        public void UpdateTypeText()
        {
            if (text == null)
            {
                m_TypeLocalizedText = TypeLocalizedText.None;
                if (gameObject.GetComponent<Text>() != null || gameObject.GetComponent<TextMeshProUGUI>() != null)
                {
                    text = gameObject;
                    UpdateTypeText();
                }
                return;
            }

            if (text.GetComponent<Text>() == null && text.GetComponent<TextMeshProUGUI>() == null)
            { 
                text = null;
            }
            else
            {
                if (text.GetComponent<TextMeshProUGUI>() != null)
                {
                    m_TypeLocalizedText = TypeLocalizedText.TextMeshProUGUI;
                }
                else if (text.GetComponent<Text>() != null)
                {
                    m_TypeLocalizedText = TypeLocalizedText.Text;
                }
            }
        }
    }
}
