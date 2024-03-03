using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class DealDamageAroundOwnerNonTargetAbility : NonTargetAbility, IAbleToDealDamage, IAbleToReturnRange
    {
        [SerializeField] protected int radius;
        [SerializeField] protected int damage;
        [SerializeField] protected DamageType damageType;
        [SerializeField] protected List<StatEffect> additionalDebuffs;

        public override void Use()
        {
            if (!PossibleToUse())
                return;

            SpendResourcePoints();
            SetCooldownAsAfterUse();

            if(impactSoundSource != null)
                impactSoundSource.Play();

            CurrentlyActiveObjects.Add(this);
            _owner.Animator.SetTrigger(abilityAnimatorTrigger.ToString());
            StartCoroutine(ApplyEffect(timeBeforeImpact));
            Invoke(nameof(RemoveCurremtlyActive), fullAnimationTime);
        }

        protected virtual IEnumerator ApplyEffect(float time)
        {
            yield return new WaitForSeconds(time);
            EntryPoint.Instance.PathFinder.GetCellsAreaForAOE(_owner.CurrentCell, radius, true, false)
                .Where(cell => cell.Content != null).ToList()
                .ForEach(cell => 
                { 
                    cell.Content.Health.TakeDamage(CalculateDamage(), null);
                    
                    foreach(StatEffect effect in additionalDebuffs)
                    {
                        cell.Content.Stats.AddStatEffect(new StatEffect(effect));
                    }
                });
        }

        private void RemoveCurremtlyActive() => CurrentlyActiveObjects.Remove(this);

        public int GetDefaultDamage() => damage;

        public DamageType GetDamageType() => damageType;

        public int CalculateDamage() => Extensions.CalculateOutgoingDamageWithGameRules(damage, damageType, _owner.Stats);

        public int ReturnRange() => radius;
    }
}