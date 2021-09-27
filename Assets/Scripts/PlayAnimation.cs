using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayAnimation : CurrentlyActiveBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip animationClip;
    [SerializeField] private AnimationClip idleAnimation;
    public UnityEvent OnAnimationPlayed;

    public void Play()
    {
        animator.Play(animationClip.name);
        ActiveNow = true;
    }

    public void Update()
    {
        //checks if THIS animation was played last + ANY animation already played
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == animationClip.name && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9)
        {
            animator.Play(idleAnimation.name);
            ActiveNow = false;
            OnAnimationPlayed.Invoke();
        }
    }
}
