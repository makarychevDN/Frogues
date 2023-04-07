using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class HotKey : MonoBehaviour
    {
        [SerializeField] private KeyCode keyCode;

        void Update()
        {
            if (!Input.GetKeyDown(keyCode))
                return;

            var button = GetComponentInChildren<Button>();

            if(button != null)
                button.onClick.Invoke();
        }
    }
}