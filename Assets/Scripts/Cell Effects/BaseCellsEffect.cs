using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCellsEffect : MonoBehaviour
{
    public abstract void ApplyEffect();
    public abstract void ApplyEffect(List<Cell> cells);
}
