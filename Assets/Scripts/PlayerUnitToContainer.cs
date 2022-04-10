using UnityEngine;

namespace FroguesFramework
{
    public class PlayerUnitToContainer : MonoBehaviour
    {
        [SerializeField] private UnitContainer unitContainer;

        void Start()
        {
            unitContainer.Content = FindObjectOfType<PlayerInput>().GetComponentInParent<Unit>();
        }
    }
}