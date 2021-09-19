using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageOnCelectedCells : BaseCellsEffect
{
    [SerializeField] private BaseCellsTaker cellsTaker;
    [SerializeField] private IntContainer damage;
    [SerializeField] private DamageTypeContainer damageType;

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
            if (!cell.IsEmpty && cell.Content.health != null)
            {
                cell.Content.health.TakeDamage(damage.Content, damageType.Content);
            }
        }
    }
}
