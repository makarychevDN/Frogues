using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class AkternatesRunFromTargetAndSupportEnemiesAI : MonoBehaviour, IAbleToAct
    {
        [SerializeField] private Unit target;
        [SerializeField] private UnitTargetAbility supportUnitAbilty;
        private Unit _unit;
        private bool _moveFromTargetMode;

        public void Act()
        {
            if (_moveFromTargetMode)
            {
                TryToRunFromTarget();
            }
            else
            {
                TryToSupportEnemy();
            }
        }

        private void TryToSupportEnemy()
        {
            if (supportUnitAbilty == null || !supportUnitAbilty.IsResoursePointsEnough())
            {
                EndTurn();
                return;
            }

            var enemies = CellsTaker.TakeAllUnits();
            enemies = enemies.Where(unit => unit.IsEnemy).ToList();
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

        private void TryToRunFromTarget()
        {
            if (_unit.MovementAbility == null || !_unit.MovementAbility.IsResoursePointsEnough())
            {
                EndTurn();
                return;
            }

            var mostFarCells = new List<Cell>() { _unit.CurrentCell };
            var neighborCells = CellsTaker.TakeCellsAreaByRange(_unit.CurrentCell, 1).EmptyCellsOnly();
            var farestDistance = target.CurrentCell.DistanceToCell(_unit.CurrentCell);

            foreach (var cell in neighborCells)
            {
                if (target.CurrentCell.DistanceToCell(cell) > farestDistance)
                {
                    mostFarCells.Clear();
                    farestDistance = target.CurrentCell.DistanceToCell(cell);
                }

                if (target.CurrentCell.DistanceToCell(cell) == farestDistance)
                    mostFarCells.Add(cell);
            }

            if (mostFarCells.Contains(_unit.CurrentCell))
            {
                EndTurn();
                return;
            }

            _unit.MovementAbility.CalculateUsingArea();
            _unit.MovementAbility.UseOnCells(new List<Cell> { mostFarCells.GetRandomElement() });
        }

        private void EndTurn()
        {
            _unit.AbleToSkipTurn.AutoSkip();
            _moveFromTargetMode = !_moveFromTargetMode;
        }

        public void Init()
        {
            _unit = GetComponentInParent<Unit>();

            if (target == null)
                target = EntryPoint.Instance.MetaPlayer;
        }
    }
}