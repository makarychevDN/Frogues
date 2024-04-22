using UnityEngine;

namespace FroguesFramework
{
    public class UnitsUIEnabler : MonoBehaviour
    {
        [SerializeField] private GameObject uiParent;

        private void Update()
        {
            uiParent.SetActive(EntryPoint.Instance.NeedToShowUnitsUI);
        }
    }
}