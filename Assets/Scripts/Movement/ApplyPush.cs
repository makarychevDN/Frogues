using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyPush : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] private Movable movable;
    [SerializeField] private Vector2IntContainer lastTakenDireaction;
    [SerializeField, Range(0.1f, 30)] private float pushSpeed;

    public void Apply()
    {
        movable.Move(Map.Instance.FindNeigborhoodForCell(unit.currentCell, lastTakenDireaction.Content), 0, pushSpeed, 0);
    }
}
