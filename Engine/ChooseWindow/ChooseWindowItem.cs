using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Engine
{
    [AddComponentMenu("Engine/Choose Window/Choose Item")]
    public class ChooseWindowItem : MonoBehaviour
    {
        private ChooseWindow chooseWindow;
        [SerializeField] private GameObject activeIfSelect;

        public void Init(ChooseWindow chooseWindow)
        {
            this.chooseWindow = chooseWindow;

            GetComponentInChildren<Button>().onClick.AddListener(this.OnClick);

            activeIfSelect.SetActive(false);
        }

        private void OnClick()
        {
            this.chooseWindow.ChangeIndex(int.Parse(this.name));
        }

        public void SetSelected()
        {
            activeIfSelect.SetActive(true);
        }

        public void SetDeselected()
        {
            activeIfSelect.SetActive(false);
        }
    }
}
