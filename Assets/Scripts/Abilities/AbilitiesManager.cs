using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class AbilitiesManager : MonoBehaviour
    {
        [SerializeField] private int weaponDamage;
        [SerializeField] private int weaponActionPointsCost;
        [SerializeField] private List<BaseAbility> _abilities = new();
        public UnityEvent<BaseAbility> AbilityHasBeenAdded;
        public UnityEvent<BaseAbility> AbilityHasBeenRemoved;
        public UnityEvent OnWeaponsDamageUpdated;
        private IAbleToHaveCurrentAbility _ableToHaveCurrentAbility;
        [SerializeField] private List<BattleStanceAbility> _battleStanceAbilities = new();
        public UnityEvent OnWeaponChanged;

        public IAbleToHaveCurrentAbility AbleToHaveCurrentAbility => _ableToHaveCurrentAbility;
        public List<BaseAbility> Abilities => _abilities;
        public int WeaponDamage => weaponDamage;
        public int WeaponActionPointsCost => weaponActionPointsCost;

        public void Init(Unit unit)
        {
            _ableToHaveCurrentAbility = unit.ActionsInput as IAbleToHaveCurrentAbility;

            foreach (var ability in GetComponentsInChildren<BaseAbility>())
            {
                ability.Init(unit);
            }
        }
        
        public void AddAbility(BaseAbility ability)
        {
            if (ability is MovementAbility)
                return;

            var nativeAttack = ability as IAbleToBeNativeAttack;
            if (nativeAttack != null && nativeAttack.IsNativeAttack() && _ableToHaveCurrentAbility is IAbleToHaveNativeAttack)
            {
                (_ableToHaveCurrentAbility as IAbleToHaveNativeAttack).SetCurrentNativeAttack(nativeAttack);
            }

            _abilities.Add(ability);
            (ability).transform.parent = transform;
            AbilityHasBeenAdded.Invoke(ability);

            if (ability is not BattleStanceAbility)
                return;

            var stanceAbility = ability as BattleStanceAbility;
            _battleStanceAbilities.Add(stanceAbility);
            stanceAbility.OnThisStanceSelected.AddListener(StanceUpdated);
        }

        private void StanceUpdated(BattleStanceAbility enabledStanceAbility)
        {
            foreach(var stance in _battleStanceAbilities)
            {
                if (stance != enabledStanceAbility && stance.StanceActiveNow)
                {                    
                    stance.ApplyEffect(false);
                }
            }
        }

        public void RemoveAbility(BaseAbility ability)
        {
            _abilities.Remove(ability);
            AbilityHasBeenRemoved.Invoke(ability);

            if (ability is not BattleStanceAbility)
                return;

            var stanceAbility = ability as BattleStanceAbility;
            _battleStanceAbilities.Remove(stanceAbility);
            stanceAbility.OnThisStanceSelected.RemoveListener(StanceUpdated);
        }
        
        public void RemoveAllWeaponAbilities()
        {
            foreach(var abilityToRemove in _abilities.Where(ability => ability.IsPartOfWeapon).ToList())
            {
                abilityToRemove.UnInit();
                RemoveAbility(abilityToRemove);
            }
        }

        public void InvokeOnWeaponChanged() => OnWeaponChanged.Invoke();

        public void SetWeaponDamage(int value)
        {
            weaponDamage = value;
        }

        public void SetWeaponActionPointsCost(int value)
        {
            weaponActionPointsCost = value;
        }
    }
}