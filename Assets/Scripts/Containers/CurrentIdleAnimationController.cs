using System;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class CurrentIdleAnimationController : MonoBehaviour
    {
        [SerializeField] private AnimationClipContainer currentIdleAnimationClipContainer;
        [SerializeField] private AbilitiesOfPlayerController abilitiesOfPlayerController;
        [SerializeField] private AnimationClip weaponlessIdleAnimation;
        [SerializeField] private List<AbilityAndAnimation> abilitiesAndAnimations;
        [SerializeField] private Animator animator;

        private void Start()
        {
            abilitiesOfPlayerController.OnAbilitiesUpdated.AddListener(UpdateIdleAnimation);
        }

        private void UpdateIdleAnimation()
        {
            currentIdleAnimationClipContainer.Content = weaponlessIdleAnimation;

            foreach (var abilityAndAnimation in abilitiesAndAnimations)
            {
                if (!abilitiesOfPlayerController.ContainsAbility(abilityAndAnimation.ability))
                    continue;

                currentIdleAnimationClipContainer.Content = abilityAndAnimation.idleAnimation;
                animator.Play(currentIdleAnimationClipContainer.Content.name);
                return;
            }
        }
        
        [Serializable]
        private struct AbilityAndAnimation
        {
            public Ability ability;
            public AnimationClip idleAnimation;
        }
    }
}