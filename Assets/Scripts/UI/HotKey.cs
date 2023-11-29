using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class HotKey : MonoBehaviour
    {
        [SerializeField] private KeyCode currentKeyCode;
        [SerializeField] private List<TMP_Text> labels;
        private bool isAbleToInput;
        private Dictionary<KeyCode, string> stringsByKeyCodes = new Dictionary<KeyCode, string>
        {
            { KeyCode.Alpha1, "1" },
            { KeyCode.Alpha2, "2" },
            { KeyCode.Alpha3, "3" },
            { KeyCode.Alpha4, "4" },
            { KeyCode.Alpha5, "5" },
            { KeyCode.Alpha6, "6" },
            { KeyCode.Alpha7, "7" },
            { KeyCode.Alpha8, "8" },
            { KeyCode.Alpha9, "9" },
            { KeyCode.Alpha0, "0" },
            { KeyCode.Minus, "-" },
            { KeyCode.Equals, "=" }
        };

        void Update()
        {
            if (!isAbleToInput)
                return;

            if (!Input.GetKeyDown(currentKeyCode))
                return;

            var button = GetComponentInChildren<Button>();

            if(button != null)
                button.onClick.Invoke();
        }

        public void SetKeyCode(KeyCode keyCode)
        {
            currentKeyCode = keyCode;

            labels.ForEach(label => label.text = stringsByKeyCodes[keyCode]);
        }

        public void EnableHotKey(bool value)
        {
            isAbleToInput = value;
            labels.ForEach(label => label.enabled = value);
        }


    }
}