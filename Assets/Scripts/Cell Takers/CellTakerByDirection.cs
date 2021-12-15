using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CellTakerByDirection : BaseCellsTaker
{
    [SerializeField] protected Vector2IntContainer currentDirectionContainer;

    public abstract List<Cell> Take(Vector2Int direction);

}
