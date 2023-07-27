using System.Collections.Generic;

namespace FroguesFramework
{
    public abstract class AreaTargetAbility : AnyTargetAbility, IAbleToUseOnCells
    {
        public abstract List<Cell> SelectCells(List<Cell> cells);
        public abstract bool PossibleToUseOnCells(List<Cell> cells);
        public abstract void UseOnCells(List<Cell> cells);
        public abstract void VisualizePreUseOnCells(List<Cell> cells);
    }
}