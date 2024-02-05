using UnityEngine;

namespace FroguesFramework
{
    public class AlternatesRunFromTargetAndHitTarget : AlternatesRunFromTargetAndDoSomethingAI
    {
        [SerializeField] private UnitTargetAbility hitTargetAbilty;

        private void TryToHitEnemy()
        {
            if (hitTargetAbilty == null || !hitTargetAbilty.IsResoursePointsEnough())
            {
                EndTurn();
                return;
            }

            hitTargetAbilty.PrepareToUsing(target);
            if (!hitTargetAbilty.PossibleToUseOnUnit(target))
            {
                EndTurn();
                return;
            }

            hitTargetAbilty.UseOnUnit(target);
        }

        protected override void TryToDoSomething() => TryToHitEnemy();
    }
}