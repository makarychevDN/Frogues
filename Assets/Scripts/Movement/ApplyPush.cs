using UnityEngine;

namespace FroguesFramework
{
    public class ApplyPush : MonoBehaviour
    {
        [SerializeField] private Unit unit;
        [SerializeField] private Movable movable;
        [SerializeField] private Vector2IntContainer lastTakenDireaction;
        [SerializeField, Range(0.1f, 30)] private float pushSpeed;

        public void Apply()
        {
            movable.Move(Map.Instance.FindNeighborhoodForCell(unit.currentCell, lastTakenDireaction.Content), 0,
                pushSpeed, 0);
        }
    }
}