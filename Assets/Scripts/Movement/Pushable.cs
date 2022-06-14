using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class Pushable : MonoBehaviour
    {
        [SerializeField] private Unit unit;
        [SerializeField, ReadOnly] private HexDirContainer lastPushDirection;
        [SerializeField, ReadOnly] private HexDirContainer PrePushDirection;

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
            PrePushDirection.Content = HexDir.zero;
        }

        private HexDir CalculateMovementVector(Cell pusherCell)
        {
            return unit.currentCell.GetComponent<HexagonCellNeighbours>().OppositeDirsByCell[pusherCell];
        }
    }
}