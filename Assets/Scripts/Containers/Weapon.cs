using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapon : CostsActionPointsBehaviour
{
    [SerializeField] private List<BaseCellsEffect> cellEffects;
    [SerializeField] private BaseCellsTaker validCellTaker;
    [SerializeField] private BaseCellsTaker selectedCellTaker;

    public void Use()
    {
        var cells = selectedCellTaker.Take().Where(selectedCell => validCellTaker.Take().Contains(selectedCell)).ToList();
        cellEffects.ForEach(effect => effect.ApplyEffect(cells));
    }
}
