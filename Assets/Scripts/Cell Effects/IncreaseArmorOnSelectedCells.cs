using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IncreaseArmorOnSelectedCells : BaseCellsEffect
{
    public override void ApplyEffect()
    {
        throw new System.NotImplementedException();
    }

    public override void ApplyEffect(List<Cell> cells)
    {
        cells.Where(cell => cell.Content != null).Where(cell => cell.Content.health != null).ToList().ForEach(cell => cell.Content.health.Armor.Content++);
    }
}
