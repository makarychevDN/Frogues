using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : CurrentlyActiveBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip animationClip;
    [SerializeField] private AnimationClip defaultAnimatorState;

    public void Play()
    {
        print("hello world");
        ActiveNow = true;
        animator.Play(animationClip.name);
    }

    public void Update()
    {
        //check animation already played
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            animator.Play(defaultAnimatorState.name);
            ActiveNow = false;
        }
    }
}
