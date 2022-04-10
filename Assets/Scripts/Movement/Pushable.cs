using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class Pushable : MonoBehaviour
    {
        [SerializeField] private Unit unit;
        [SerializeField, ReadOnly] private Vector2IntContainer lastPushDirection;
        [SerializeField, ReadOnly] private Vector2IntContainer PrePushDirection;

        public UnityEvent OnPushed;
        public UnityEvent OnPrepushed;

        public void Push(Cell pusherCell)
        {
            lastPushDirection.Content = CalculateMovementVector(pusherCell);
            OnPushed.Invoke();
        }

        public void PrePush(Cell pusherCell)
        {
            PrePushDirection.Content = CalculateMovementVector(pusherCell);
            OnPrepushed.Invoke();
        }

        public void ResetPrePushValue()
        {
            PrePushDirection.Content = Vector2Int.zero;
        }

        private Vector2Int CalculateMovementVector(Cell pusherCell)
        {
            return (unit.Coordinates - pusherCell.coordinates).ToVector3().normalized.ToVector2Int();
        }
    }
}