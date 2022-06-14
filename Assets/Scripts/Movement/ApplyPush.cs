using UnityEngine;

namespace FroguesFramework
{
    public class ApplyPush : MonoBehaviour
    {
        [SerializeField] private Unit unit;
        [SerializeField] private Movable movable;
        [SerializeField] private HexDirContainer lastTakenDirection;
        [SerializeField, Range(0.1f, 30)] private float pushSpeed;

        public void Apply()
        {
            movable.Move(unit.currentCell.GetComponent<HexagonCellNeighbours>().Neighbours[lastTakenDirection.Content], 0,
                pushSpeed, 0);
        }
    }
}