using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class UpdateTextMeshProByButtonIsInteractable : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmpText;
        [SerializeField] private Button button;
        [SerializeField] private string textOnButtonIsInteractable;
        [SerializeField] private string textOnButtonIsUninteractable;
        private bool _hashedState;

        void Update()
        {
            if(_hashedState == button.interactable)
                SetTextToTmpText(button.interactable ?  textOnButtonIsInteractable : textOnButtonIsUninteractable);

            _hashedState = button.interactable;
        }

        private void SetTextToTmpText(string text) => tmpText.text = text;
    }
}