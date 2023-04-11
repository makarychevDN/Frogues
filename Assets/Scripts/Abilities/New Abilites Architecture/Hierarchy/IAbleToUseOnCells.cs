using System.Collections.Generic;

namespace FroguesFramework
{
    public interface IAbleToUseOnCells
    {
        public bool PossibleToUseOnUnit(List<Cell> cells);
        public void UseOnUnit(List<Cell> cells);
    }
}