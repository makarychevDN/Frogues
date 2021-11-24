
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CellsEffectWithPreVisualization : BaseCellsEffect
{    
    public abstract void PreVisualizeEffect();
    public abstract void PreVisualizeEffect(List<Cell> cells);
}
