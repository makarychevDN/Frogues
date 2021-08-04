using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : Container<Unit>
{
    public MapLayer _mapLayer;
    public Vector2Int _coordinates;
    public override Unit Content 
    { 
        get => base.Content;
        set
        {
            base.Content = value;
            if(value != null)
            {
                value._currentCell = this;
            }
        }
    }
}
