using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToIdleAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip idleAnimation;

    public void Return()
    {
        animator.Play(idleAnimation.name);
    }
}
