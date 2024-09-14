using UnityEngine;

namespace FroguesFramework
{
    public class UnitsUIEnabler : MonoBehaviour
    {
        [SerializeField] private GameObject uiParent;
        [SerializeField] private Unit owner;

        private void Update()
        {
            if(owner.CurrentRoom != null)
            uiParent.SetActive(owner.CurrentRoom.NeedToShowUnitsUI);
        }
    }
}