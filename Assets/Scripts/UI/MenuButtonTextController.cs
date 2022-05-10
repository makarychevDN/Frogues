using TMPro;
using UnityEngine;

namespace FroguesFramework
{
    public class MenuButtonTextController : MonoBehaviour
    {
        [SerializeField] private string text;
        [SerializeField] private TMP_Text mainTextBox;
        [SerializeField] private TMP_Text shadowTextBox;

        private void OnValidate()
        {
            if (mainTextBox != null)
                mainTextBox.text = text;
            
            if (shadowTextBox != null)
                shadowTextBox.text = text;
        }
    }
}