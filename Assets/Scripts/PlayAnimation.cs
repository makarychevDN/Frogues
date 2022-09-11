using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class PlayAnimation : CurrentlyActiveBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private AnimationClipContainer animationClipContainer;
        [SerializeField] private AnimationClipContainer idleAnimationContainer;
        public UnityEvent OnAnimationPlayed;

        public void Play()
        {
            animator.Play(animationClipContainer.Content.name);
            ActiveNow = true;
        }

        public void Update()
        {
            if(animationClipContainer == null || animationClipContainer.Content == null)
                return;
            
            //checks if THIS animation was played last + ANY animation already played
            if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == animationClipContainer.Content.name &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
            {
                animator.Play(idleAnimationContainer.Content.name);
                ActiveNow = false;
                OnAnimationPlayed.Invoke();
            }
        }
    }
}
