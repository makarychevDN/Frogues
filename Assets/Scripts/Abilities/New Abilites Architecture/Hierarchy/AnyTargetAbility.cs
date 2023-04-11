using System.Collections.Generic;

namespace FroguesFramework
{
    public abstract class AnyTargetAbility : AbleToCostAbility, IAbleToCalculateUsingArea, IAbleToDisablePreVisualization
    {
        protected List<Cell> _usingArea;

        public abstract List<Cell> CalculateUsingArea();

        public abstract void DisablePreVisualization();
    }
}