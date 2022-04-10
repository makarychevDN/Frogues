using UnityEngine;

namespace FroguesFramework
{
    public class AllAbilityButtonsMaterialEnabler : MonoBehaviour
    {
        public void Enable(bool value)
        {
            //FindObjectsOfType<AbilityButton>().ToList().ForEach(button => button.UsingNow = value);
        }

        private void Start()
        {
            Enable(false);
        }
    }
}