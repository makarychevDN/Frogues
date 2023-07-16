using UnityEngine;

namespace FroguesFramework
{
    [RequireComponent(typeof(Animator))]
    public class AnimatedWall : MonoBehaviour
    {
        [SerializeField] private bool showed = true;
        [SerializeField] private Animator animator;
        private float _cooldown = 0.2f;
        private bool _coldowned = true;

        public bool Showed
        {
            get => showed;
            set 
            {
                if (!_coldowned)
                    return;

                _coldowned = false;
                Invoke(nameof(ResetCooldown), _cooldown);
                showed = value;
                animator.SetTrigger(value ? "Show" : "Hide");
            }
        }

        private void ResetCooldown() => _coldowned = true;
    }
}