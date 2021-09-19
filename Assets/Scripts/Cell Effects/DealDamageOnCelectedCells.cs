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
        print(0);
        foreach (var cell in CellsListToCulumnsList(cells))
        {
            print(1);
            if (!cell.IsEmpty && cell.Content.health != null)
            {
                print(2);
                cell.Content.health.TakeDamage(damage.Content, damageType.Content);
            }
        }
    }
}
