using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class ShapelessnessAbility : JumpOnCellAbility, IAbleToDealDamage
    {
        [SerializeField] private int damage;
        [SerializeField] private DamageType damageType;
        private List<Unit> _hashedUnitTargets;

        public override void UseOnCells(List<Cell> cells)
        {
            if (!PossibleToUseOnCells(cells))
                return;

            _hashedUnitTargets = CalculateTargets(cells[0], CellsTaker.GetDirByStartAndEndCells(_owner.CurrentCell, cells[0]));
            base.UseOnCells(cells);
            _owner.Movable.OnMovementEnd.AddListener(DealDamage);
        }

        public override List<Cell> CalculateUsingArea()
        {
            return _usingArea = CellsTaker.TakeCellsLinesInAllDirections(_owner.CurrentCell, CellsTaker.ObstacleMode.everyUnitIsObstacle, false, false).ToList();
        }

        private List<Unit> CalculateTargets(Cell targetCell, HexDir hexDir)
        {
            List<Cell> line = new List<Cell>();

            Cell cellIterator = _owner.CurrentCell;
            while (true)
            {
                cellIterator = cellIterator.CellNeighbours.GetNeighborByHexDir(hexDir);

                if (cellIterator == targetCell)
                    break;

                line.Add(cellIterator);
            }

            return line.ContentFromEachCellWioutNulls();
        }

        public override void VisualizePreUseOnCells(List<Cell> cells)
        {
            base.VisualizePreUseOnCells(cells);

            if (!PossibleToUseOnCells(cells))
                return;

            var preDamagedTargets = CalculateTargets(cells[0], CellsTaker.GetDirByStartAndEndCells(_owner.CurrentCell, cells[0]));
            PreDealDamage(preDamagedTargets);
        }

        private void DealDamage()
        {
            _hashedUnitTargets.ForEach(unit => unit.Health.TakeDamage(CalculateDamage(), _owner));
            _owner.Movable.OnMovementEnd.RemoveListener(DealDamage);
            impactSoundSource?.Play();
        }

        private void PreDealDamage(List<Unit> units)
        {
            units.ForEach(unit => unit.Health.PreTakeDamage(CalculateDamage(), _owner));
        }

        public int CalculateDamage() => Extensions.CalculateDamageWithGameRules(damage, damageType, _owner.Stats);

        public int GetDefaultDamage() => damage;

        public DamageType GetDamageType() => damageType;
    }
}