using System;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class CurrentIdleAnimationController : MonoBehaviour
    {
        [SerializeField] private AnimationContainer idleAnimationContainer;
        [SerializeField] private AbilitiesOfPlayerController abilitiesOfPlayerController;
        [SerializeField] private AnimationClip weaponlessIdleAnimation;
        [SerializeField] private List<AbilityAndAnimation> abilitiesAndAnimations;

        private void Start()
        {
            abilitiesOfPlayerController.OnAbilitiesUpdated.AddListener(UpdateIdleAnimation);
        }

        private void UpdateIdleAnimation()
        {
            idleAnimationContainer.Content = weaponlessIdleAnimation;

            foreach (var abilityAndAnimation in abilitiesAndAnimations)
            {
                if (!abilitiesOfPlayerController.ContainsAbility(abilityAndAnimation.ability))
                    continue;

                idleAnimationContainer.Content = abilityAndAnimation.idleAnimation;
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