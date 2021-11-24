using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Pusher : CellsEffectWithPreVisualization
{
    [SerializeField] private Unit unit;
    [SerializeField] private BaseCellsTaker cellsTaker;
    private List<Cell> columnCells = new List<Cell>();
    
    public UnityEvent OnPush;

    public override void ApplyEffect() => ApplyEffect(cellsTaker.Take());

    public override void ApplyEffect(List<Cell> cells)
    {

        if (cells == null)
            return;

        TakeCellsAbleToBePushed(cells).ForEach(cell => cell.Content.pushable.Push(unit.currentCell));
    }

    public override void PreVisualizeEffect() => PreVisualizeEffect(cellsTaker.Take());

    public override void PreVisualizeEffect(List<Cell> cells)
    {
        if (cells == null)
            return;

        TakeCellsAbleToBePushed(cells).ForEach(cell => cell.Content.pushable.PrePush(unit.currentCell));
    }

    private List<Cell> TakeCellsAbleToBePushed(List<Cell> cells)
    {
        return CellsListToCulumnsList(cells).Where(cell => !cell.IsEmpty && cell.Content.pushable != null).ToList();
    }
}

