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

            hitTargetAbilty.PrepareToUsing(_target);
            if (!hitTargetAbilty.PossibleToUseOnUnit(_target))
            {
                EndTurn();
                return;
            }

            hitTargetAbilty.UseOnUnit(_target);
        }

        protected override void TryToDoSomething() => TryToHitEnemy();
    }
}