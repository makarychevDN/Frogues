using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class AbilitiesManager : MonoBehaviour
    {
        private IAbleToHaveCurrentAbility _ableToHaveCurrentAbility;
        private List<BaseAbility> _abilities = new();
        public UnityEvent<BaseAbility> AbilityHasBeenAdded;
        public UnityEvent<BaseAbility> AbilityHasBeenRemoved;

        public IAbleToHaveCurrentAbility AbleToHaveCurrentAbility => _ableToHaveCurrentAbility;

        public void Init(Unit unit)
        {
            _ableToHaveCurrentAbility = unit.ActionsInput as IAbleToHaveCurrentAbility;

            foreach (var ability in GetComponentsInChildren<IAbility>())
            {
                ability.Init(unit);
            }
        }
        
        public void AddAbility(BaseAbility ability)
        {
            if (ability is NewMovementAbility)
                return;

            _abilities.Add(ability);
            ((MonoBehaviour)ability).transform.parent = transform;
            AbilityHasBeenAdded.Invoke(ability);
        }

        public void RemoveAbility(BaseAbility ability)
        {
            _abilities.Remove(ability);
            AbilityHasBeenRemoved.Invoke(ability);
        }
        
        public void RemoveAllWeaponAbilities()
        {
            //var abilitiesToRemove = _abilities.Where(ability => ability.IsPartOfWeapon()).ToList();
            //abilitiesToRemove.ForEach(abilityToRemove => RemoveAbility(abilityToRemove));
        }
    }
}