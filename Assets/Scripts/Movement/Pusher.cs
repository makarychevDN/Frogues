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
        foreach (var cell in cellsTaker.Take())
        {
            if (cell.Content.pushable != null)
            {
                cell.Content.pushable.Push(unit.currentCell);
            }
        }
    }
}
