using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class AbilitiesManager : MonoBehaviour
    {
        [SerializeField] private IAbleToHaveCurrentAbility _ableToHaveCurrentAbility;
        private List<IAbility> _abilities = new();
        public UnityEvent<IAbility> AbilityHasBeenAdded;
        public UnityEvent<IAbility> AbilityHasBeenRemoved;

        public IAbleToHaveCurrentAbility AbleToHaveCurrentAbility => _ableToHaveCurrentAbility;

        public void Init(Unit unit)
        {
            _ableToHaveCurrentAbility = unit.ActionsInput as IAbleToHaveCurrentAbility;
        }
        
        public void AddAbility(IAbility ability)
        {
            _abilities.Add(ability);
            ((MonoBehaviour)ability).transform.parent = transform;
            AbilityHasBeenAdded.Invoke(ability);
        }

        public void RemoveAbility(IAbility ability)
        {
            _abilities.Remove(ability);
        }
    }
}