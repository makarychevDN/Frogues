using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class InspectAbility : UnitTargetAbility, IAbleToReturnIsPrevisualized
    {
        [SerializeField] private bool showMovementHighlighting;
        private Unit _hashedTarget;
        private bool _isPrevisualizedNow;

        public bool ShowMovementHighlighting
        {
            get { return showMovementHighlighting; }
            set { showMovementHighlighting = value; }
        }

        public override int CalculateHashFunctionOfPrevisualisation()
        {
            int hash = 0;
            if(_hashedTarget != null)
                hash ^= _hashedTarget.GetHashCode();

            return hash ^ GetHashCode();
        }

        public override List<Cell> CalculateUsingArea()
        {
            return _usingArea = CellsTaker.TakeAllCells();
        }

        public override void DisablePreVisualization() => _isPrevisualizedNow = false;

        public override bool PossibleToUseOnUnit(Unit target)
        {
            return target != null;
        }

        public override void PrepareToUsing(Unit target)
        {
            _hashedTarget = target;
        }

        public override void UseOnUnit(Unit target)
        {
            if(target == null) 
                return;

            EntryPoint.Instance.UnitDescriptionPanel.Activate(target);
            target.OnInspectIt.Invoke();
        }

        public override void VisualizePreUseOnUnit(Unit target)
        {
            _isPrevisualizedNow = true;

            if (showMovementHighlighting)
            {
                if (_owner.Stats.Immobilized == 0)
                {
                    var movementCells = EntryPoint.Instance.PathFinder.GetCellsAreaByActionPoints(_owner.CurrentCell,
                        _owner.ActionPoints.AvailablePoints,
                        _owner.MovementAbility.GetActionPointsCost(), false, true, true);
                    movementCells.ForEach(cell => cell.EnableValidForMovementCellHighlight(movementCells));
                }
                   
            }

            if (!PossibleToUseOnUnit(target))
                return;

            target.MaterialInstanceContainer.EnableOutline(true);
        }

        public override bool IsIgnoringDrawingFunctionality() => false;

        public override bool CheckItUsableOnBloodSurfaceUnit() => false;

        public override bool CheckItUsableOnDefaultUnit() => true;

        public bool IsPrevisualizedNow() => _isPrevisualizedNow;
    }
}
