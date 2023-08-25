using UnityEngine;

namespace FroguesFramework
{
    [RequireComponent(typeof(Canvas))]
    public class UnitsUIEnabler : MonoBehaviour
    {
        private Canvas canvas;

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
        }

        private void Update()
        {
            canvas.enabled = EntryPoint.Instance.NeedToShowUnitsUI;
        }
    }
}