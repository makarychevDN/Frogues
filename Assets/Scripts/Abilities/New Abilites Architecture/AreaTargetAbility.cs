using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public abstract class AreaTargetAbility : AnyTargetAbility, IAbleToUseOnCells
    {
        public abstract bool PossibleToUseOnUnit(List<Cell> cells);
        public abstract void UseOnUnit(List<Cell> cells);
    }
}