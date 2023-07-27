using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FroguesFramework
{
    public interface IAbleToUseInDirectionOfCursorPosition
    {
        public List<Cell> SelectCells(Vector3 cursorPosition);
        public bool PossibleToUseInDirection(Vector3 cursorPosition);
        public void UseInDirection(Vector3 cursorPosition);
        public void VisualizePreUseInDirection(Vector3 cursorPosition);
    }
}