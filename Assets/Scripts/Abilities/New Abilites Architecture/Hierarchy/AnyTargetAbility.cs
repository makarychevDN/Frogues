using System.Collections.Generic;

namespace FroguesFramework
{
    public abstract class AnyTargetAbility : AbleToCostAbility, IAbleToCalculateUsingArea, IAbleToVisualizePreUse
    {
        protected List<Cell> _usingArea;

        public abstract List<Cell> CalculateUsingArea();
        public abstract void VisualizePreUse();
        public abstract void DisablePreUseVisualization();
    }
}