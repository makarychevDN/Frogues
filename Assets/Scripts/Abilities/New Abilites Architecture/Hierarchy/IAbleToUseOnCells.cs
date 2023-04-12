using System.Collections.Generic;

namespace FroguesFramework
{
    public interface IAbleToUseOnCells
    {
        public List<Cell> SelectCells(List<Cell> cells);
        public bool PossibleToUseOnCells(List<Cell> cells);
        public void UseOnCells(List<Cell> cells);
        public void VisualizePreUseOnCells(List<Cell> cells);
    }
}