using UnityEngine;

namespace FroguesFramework
{
    public class ReturnToIdleAnimation : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private AnimationClip idleAnimation;

        public void Return()
        {
            animator.Play(idleAnimation.name);
        }
    }
}