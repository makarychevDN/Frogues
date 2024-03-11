using System.Collections;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class ThunderStrikeAbility : DealDamageAroundOwnerNonTargetAbility, IAbleToDealAlternativeDamage
    {
        [SerializeField] private int alternativeDamageOnCertainRange;
        [SerializeField] private int certainRange;

        public int CalculateAlternativeDamage() => Extensions.CalculateOutgoingDamageWithGameRules(alternativeDamageOnCertainRange, damageType, _owner.Stats);

        public DamageType GetAlternativeDamageType() => damageType;

        public int GetDefaultAlternativeDamage() => alternativeDamageOnCertainRange;

        protected override IEnumerator ApplyEffect(float time)
        {
            yield return new WaitForSeconds(time);
            EntryPoint.Instance.PathFinder.GetCellsAreaForAOE(_owner.CurrentCell, radius, true, false)
                .Where(cell => cell.Content != null).ToList()
                .ForEach(cell =>
                {
                    if (cell.DistanceToCell(_owner.CurrentCell) == certainRange)
                        cell.Content.Health.TakeDamage(CalculateAlternativeDamage(), _owner);
                    else
                        cell.Content.Health.TakeDamage(CalculateDamage(), _owner);

                    foreach (StatEffect effect in additionalDebuffs)
                    {
                        cell.Content.Stats.AddStatEffect(new StatEffect(effect));
                    }
                });
        }
    }
}