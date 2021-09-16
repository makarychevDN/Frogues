using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pusher : BaseCellsEffect
{
    [SerializeField] private Unit unit;
    [SerializeField] private BaseCellsTaker cellsTaker;
    
    public UnityEvent OnPush;

    
    public override void ApplyEffect()
    {
        ApplyEffect(cellsTaker.Take());
    }

    public override void ApplyEffect(List<Cell> cells)
    {
        if (cells == null)
            return;

        foreach (var cell in cells)
        {
            if (!cell.IsEmpty && cell.Content.pushable != null)
            {
                cell.Content.pushable.Push(unit.currentCell);
            }
        }
    }
}
