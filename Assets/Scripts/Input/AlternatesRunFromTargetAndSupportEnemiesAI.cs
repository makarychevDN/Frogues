using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class AlternatesRunFromTargetAndSupportEnemiesAI  : AlternatesRunFromTargetAndDoSomethingAI
    {
        [SerializeField] private UnitTargetAbility supportUnitAbilty;

        private void TryToSupportEnemy()
        {
            if (supportUnitAbilty == null || !supportUnitAbilty.IsResoursePointsEnough())
            {
                EndTurn();
                return;
            }

            var enemies = CellsTaker.TakeAllUnits();
            enemies = enemies.Where(unit => unit.IsEnemy && !unit.Small && !unit.AbilitiesManager.Abilities.Any(ability => ability is MushroomPassiveProperty)).ToList();
            enemies.Remove(_unit);

            if (enemies.Count == 0)
            {
                EndTurn();
                return;
            }

            var randomEnemy = enemies.GetRandomElement();
            supportUnitAbilty.PrepareToUsing(randomEnemy);

            if (!supportUnitAbilty.PossibleToUseOnUnit(randomEnemy))
            {
                EndTurn();
                return;
            }

            supportUnitAbilty.UseOnUnit(randomEnemy);
        }

        protected override void TryToDoSomething() => TryToSupportEnemy();
    }
}