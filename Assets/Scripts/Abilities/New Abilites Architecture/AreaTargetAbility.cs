using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public abstract class AreaTargetAbility : AnyTargetAbility, IAbleToUseOnCells
    {
        public abstract List<Cell> SelectCells();
        public abstract bool PossibleToUseOnCells(List<Cell> cells);
        public abstract void UseOnCells(List<Cell> cells);
    }
}