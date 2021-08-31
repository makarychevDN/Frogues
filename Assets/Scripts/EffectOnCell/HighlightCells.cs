using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightCells : MonoBehaviour
{
    [SerializeField] private BaseCellsTaker _cellTaker;

    public void Apply()
    {
        MapBasedOnTilemap.Instance._layers.ForEach(layer =>
        {
            foreach(var cell in layer)
            {
                cell.EnableHighlight(false);
            }
        });

        _cellTaker.Take().ForEach(cell => { cell.EnableHighlight(true); /*cell.EnablePathDot(true);*/ });
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Apply();
    }
}
