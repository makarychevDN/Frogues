using System;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class CurrentAnimationByAbilitiesContainsController : MonoBehaviour
    {
        [SerializeField] private AnimationClipContainer currentAnimationClipContainer;
        [SerializeField] private AbilitiesOfPlayerController abilitiesOfPlayerController;
        [SerializeField] private AnimationClip weaponlessAnimation;
        [SerializeField] private List<AbilityAndAnimation> abilitiesAndAnimations;
        [SerializeField] private Animator animator;

        private void Start()
        {
            abilitiesOfPlayerController.OnAbilitiesUpdated.AddListener(UpdateIdleAnimation);
        }

        private void UpdateIdleAnimation()
        {
            currentAnimationClipContainer.Content = weaponlessAnimation;

            foreach (var abilityAndAnimation in abilitiesAndAnimations)
            {
                if (!abilitiesOfPlayerController.ContainsAbility(abilityAndAnimation.ability))
                    continue;

                currentAnimationClipContainer.Content = abilityAndAnimation.idleAnimation;
                animator.Play(currentAnimationClipContainer.Content.name);
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