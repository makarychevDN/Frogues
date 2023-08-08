using System.Collections.Generic;

namespace FroguesFramework
{
    public class InspectAbility : UnitTargetAbility
    {
        private Unit _hashedTarget;

        public override int CalculateHashFunctionOfPrevisualisation()
        {
            int hash = 0;
            if(_hashedTarget != null)
                hash ^= _hashedTarget.GetHashCode();

            return hash;
        }

        public override List<Cell> CalculateUsingArea()
        {
            return _usingArea = CellsTaker.TakeAllCells();
        }

        public override void DisablePreVisualization(){}

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
        }

        public override void VisualizePreUseOnUnit(Unit target)
        {
            if (!PossibleToUseOnUnit(target))
                return;

            target.MaterialInstanceContainer.EnableOutline(true);
            EntryPoint.Instance.UnitDescriptionPanel.Activate(target.UnitDescription.Description);
        }
    }
}
