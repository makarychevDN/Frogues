using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class ClickButtonByScript : MonoBehaviour
    {
        [SerializeField] private Button button;

        public void Click()
        {
            button.onClick.Invoke();
        }
    }
}