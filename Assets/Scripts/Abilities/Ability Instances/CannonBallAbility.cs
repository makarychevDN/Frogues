using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class CannonBallAbility : JumpOnCellAbility
    {
        [SerializeField] private int damage;
        [SerializeField] private int radiusOfDamageArea = 1;
        [SerializeField] private DamageType damageType;

        public override void UseOnCells(List<Cell> cells)
        {
            base.UseOnCells(cells);

            if (!PossibleToUseOnCells(cells))
                return;

            _owner.Movable.OnMovementEnd.AddListener(DealDamage);
        }

        public override void VisualizePreUseOnCells(List<Cell> cells)
        {
            base.VisualizePreUseOnCells(cells);

            if (!PossibleToUseOnCells(cells))
                return;

            List<Cell> targetsCells = EntryPoint.Instance.PathFinder.GetCellsAreaForAOE(_owner.CurrentCell, radiusOfDamageArea, true, false);
            targetsCells.Where(cell => cell.Content != null).ToList().ForEach(cell => cell.Content.Health.PreTakeDamage(CalculateDamage()));
        }

        private void DealDamage()
        {
            List<Cell> targetsCells = EntryPoint.Instance.PathFinder.GetCellsAreaForAOE(_owner.CurrentCell, radiusOfDamageArea, true, false);
            targetsCells.Where(cell => cell.Content != null).ToList().ForEach(cell => cell.Content.Health.TakeDamage(CalculateDamage(), null));
            _owner.Movable.OnMovementEnd.RemoveListener(DealDamage);
        }

        private int CalculateDamage() => Extensions.CalculateDamageWithGameRules(damage, damageType, _owner.Stats);
    }
}