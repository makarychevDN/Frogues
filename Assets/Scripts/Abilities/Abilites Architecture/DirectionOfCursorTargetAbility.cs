using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public abstract class DirectionOfCursorTargetAbility : AnyTargetAbility, IAbleToUseInDirectionOfCursorPosition
    {
        public abstract bool PossibleToUseInDirection(Vector3 cursorPosition);
        public abstract List<Cell> SelectCells(Vector3 cursorPosition);
        public abstract void UseInDirection(Vector3 cursorPosition);
        public abstract void VisualizePreUseInDirection(Vector3 cursorPosition);
    }
}