using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class PlayAnimation : CurrentlyActiveBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private AnimationClip animationClip;
        [SerializeField] private AnimationClipContainer idleAnimationContainer;
        public UnityEvent OnAnimationPlayed;

        public void Play()
        {
            animator.Play(animationClip.name);
            ActiveNow = true;
        }

        public void Update()
        {
            //checks if THIS animation was played last + ANY animation already played
            if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == animationClip.name &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
            {
                animator.Play(idleAnimationContainer.Content.name);
                ActiveNow = false;
                OnAnimationPlayed.Invoke();
            }
        }
    }
}
