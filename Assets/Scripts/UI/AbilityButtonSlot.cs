using UnityEngine;

namespace FroguesFramework
{
    public class AbilityButtonSlot : MonoBehaviour
    {
        [SerializeField] private Transform abilityButton;

        public void AddButton(Transform button)
        {
            abilityButton = button;
        }
    }
}