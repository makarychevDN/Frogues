using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class AbilitiesManager : MonoBehaviour
    {
        [SerializeField] private List<IAbility> abilities = new();
        public UnityEvent<IAbility> AbilityHasBeenAdded;
        public UnityEvent<IAbility> AbilityHasBeenRemoved;

        public void AddAbility(IAbility ability)
        {
            abilities.Add(ability);
            ((MonoBehaviour)ability).transform.parent = transform;
            AbilityHasBeenAdded.Invoke(ability);
        }

        public void RemoveAbility(IAbility ability)
        {
            abilities.Remove(ability);
        }
    }
}