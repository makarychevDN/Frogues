using UnityEngine;

namespace FroguesFramework
{
    public class ApplyDamage : MonoBehaviour
    {
        [SerializeField] private IntContainer hp;
        [SerializeField] private IntContainer lastTakenDamage;

        public void Apply()
        {
            hp.Content -= lastTakenDamage.Content;
        }
    }
}