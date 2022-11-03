using UnityEngine;

namespace FroguesFramework
{
    public class GameObjectSwitchIsActive : MonoBehaviour
    {
        [SerializeField] private GameObject target;

        private void Awake()
        {
            if (target == null)
                target = gameObject;
        }

        public void Switch()
        {
            target.SwitchActive();
        }
    }
}