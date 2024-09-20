using System.Collections;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class DealDamageAroundOwnerNonTargetAbility : NonTargetAbility, IAbleToDealDamage, IAbleToReturnRange
    {
        [SerializeField] protected int radius;
        [SerializeField] protected int damage;
        [SerializeField] protected DamageType damageType;

        public override void Use()
        {
            if (!PossibleToUse())
                return;

            SpendResourcePoints();
            SetCooldownAsAfterUse();

            if(impactSoundSource != null)
                impactSoundSource.Play();

            _owner.CurrentRoom.CurrentlyActiveObjects.Add(this);
            _owner.Animator.SetTrigger(abilityAnimatorTrigger.ToString());
            StartCoroutine(ApplyEffect(timeBeforeImpact));
            Invoke(nameof(RemoveCurremtlyActive), fullAnimationTime);
        }

        protected virtual IEnumerator ApplyEffect(float time)
        {
            yield return new WaitForSeconds(time);
            _owner.CurrentRoom.PathFinder.GetCellsAreaForAOE(_owner.CurrentCell, radius, true, false)
                .Where(cell => cell.Content != null).ToList()
                .ForEach(cell => 
                { 
                    if(CalculateDamage() != 0)
                        cell.Content.Health.TakeDamage(CalculateDamage(), null);                   
                });
        }

        private void RemoveCurremtlyActive() => _owner.CurrentRoom.CurrentlyActiveObjects.Remove(this);

        public int GetDefaultDamage() => damage;

        public DamageType GetDamageType() => damageType;

        public virtual int CalculateDamage() => Extensions.CalculateOutgoingDamageWithGameRules(damage, damageType, _owner.Stats);

        public int ReturnRange() => radius;
    }
}