using System.Collections.Generic;

namespace FroguesFramework
{
    public interface IAbleToUseOnCells
    {
        public List<Cell> SelectCells();
        public bool PossibleToUseOnCells(List<Cell> cells);
        public void UseOnCells(List<Cell> cells);
    }
}