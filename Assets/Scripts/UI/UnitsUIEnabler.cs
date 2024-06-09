using UnityEngine;

namespace FroguesFramework
{
    public class UnitsUIEnabler : MonoBehaviour
    {
        [SerializeField] private GameObject uiParent;
        [SerializeField] private Unit owner;

        private void Update()
        {
            uiParent.SetActive(owner.CurrentRoom.NeedToShowUnitsUI);
        }
    }
}