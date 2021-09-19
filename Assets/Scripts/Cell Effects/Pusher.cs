using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pusher : BaseCellsEffect
{
    [SerializeField] private Unit unit;
    [SerializeField] private BaseCellsTaker cellsTaker;
    private List<Cell> columnCells = new List<Cell>();
    
    public UnityEvent OnPush;

    
    public override void ApplyEffect()
    {
        ApplyEffect(cellsTaker.Take());
    }

    public override void ApplyEffect(List<Cell> cells)
    {
        if (cells == null)
            return;

        foreach (var cell in CellsListToCulumnsList(cells))
        {
            if (!cell.IsEmpty && cell.Content.pushable != null)
            {
                cell.Content.pushable.Push(unit.currentCell);
            }
        }
    }
}
