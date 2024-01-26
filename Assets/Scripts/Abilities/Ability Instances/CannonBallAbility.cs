using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class CannonBallAbility : JumpOnCellAbility, IAbleToDealDamage
    {
        [SerializeField] private int damage;
        [SerializeField] private int radiusOfDamageArea = 1;
        [SerializeField] private DamageType damageType;

        public override void UseOnCells(List<Cell> cells)
        {
            if (!PossibleToUseOnCells(cells))
                return;

            base.UseOnCells(cells);
            _owner.Movable.OnMovementEnd.AddListener(DealDamage);
        }

        public override void VisualizePreUseOnCells(List<Cell> cells)
        {
            base.VisualizePreUseOnCells(cells);

            if (!PossibleToUseOnCells(cells))
                return;

            List<Cell> targetsCells = EntryPoint.Instance.PathFinder.GetCellsAreaForAOE(cells[0], radiusOfDamageArea, true, false);
            targetsCells.Where(cell => cell.Content != null).ToList().ForEach(cell => cell.Content.Health.PreTakeDamage(CalculateDamage()));
        }

        private void DealDamage()
        {
            List<Cell> targetsCells = EntryPoint.Instance.PathFinder.GetCellsAreaForAOE(_owner.CurrentCell, radiusOfDamageArea, true, false);
            targetsCells.Where(cell => cell.Content != null).ToList().ForEach(cell => cell.Content.Health.TakeDamage(CalculateDamage(), null));
            _owner.Movable.OnMovementEnd.RemoveListener(DealDamage);
        }

        public int CalculateDamage() => Extensions.CalculateOutgoingDamageWithGameRules(damage, damageType, _owner.Stats);

        public int GetDefaultDamage() => damage;

        public DamageType GetDamageType() => damageType;
    }
}