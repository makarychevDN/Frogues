using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DealDamageOnCelectedCells : CellsEffectWithPreVisualization
{
    [SerializeField] private BaseCellsTaker cellsTaker;
    [SerializeField] private IntContainer damage;
    [SerializeField] private DamageTypeContainer damageType;
    [SerializeField] private bool ignoreArmor;

    public override void ApplyEffect() => ApplyEffect(cellsTaker.Take());

    public override void ApplyEffect(List<Cell> cells)
    {
        if (cells == null)
            return;

        TakeCellsAbleToTakeDamage(cells).ForEach(cell => cell.Content.health.TakeDamage(damage.Content, damageType.Content, ignoreArmor));
    }

    public override void PreVisualizeEffect() => PreVisualizeEffect(cellsTaker.Take());

    public override void PreVisualizeEffect(List<Cell> cells)
    {
        if (cells == null)
            return;

        TakeCellsAbleToTakeDamage(cells).ForEach(cell => cell.Content.health.PretakeDamage(damage.Content, damageType.Content, ignoreArmor));
    }

    private List<Cell> TakeCellsAbleToTakeDamage(List<Cell> cells)
    {
        return CellsListToCulumnsList(cells).Where(cell => !cell.IsEmpty && cell.Content.health != null).ToList();
    }
}
