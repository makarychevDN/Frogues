using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PrintUnitsDescriptionEffect : CellsEffectWithPreVisualization
{
    [SerializeField] private BaseCellsTaker cellTaker;
    [SerializeField] private GameObject textPanel;
    [SerializeField] private TextMeshProUGUI textField;

    public override void ApplyEffect()=> ApplyEffect(cellTaker.Take());

    public override void ApplyEffect(List<Cell> cells)
    {
        textPanel.SetActive(false);

        if (cells == null || cells.Count == 0)
            return;

        var cell = cells.FirstOrDefault(cell => !cell.IsEmpty);
        if (cell == null || cell.Content.description == null)
            return;

        textPanel.SetActive(true);
        textField.text = cell.Content.description.Content;
    }

    public override void PreVisualizeEffect() => ApplyEffect();

    public override void PreVisualizeEffect(List<Cell> cells) => ApplyEffect(cells);
}
