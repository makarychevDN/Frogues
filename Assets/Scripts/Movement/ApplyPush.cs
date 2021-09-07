using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyPush : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] private Movable movable;
    [SerializeField] private Vector2IntContainer lastTakenDireaction;

    public void Apply()
    {
        movable.Move(MapBasedOnTilemap.Instance.FindNeigborhoodForCell(unit.currentCell, lastTakenDireaction.Content), 0);
    }
}
