using UnityEngine;

namespace FroguesFramework
{
    public class SummonAndMove : MonoBehaviour
    {
        [SerializeField] private Unit spawnerUnit;
        [SerializeField] private Unit defaultUnitToSpawn;
        [SerializeField] private BaseCellsTaker cellsMoveToCellTaker;
        [SerializeField] private float movementSpeed, jumpHeight;

        public void Spawn() => Spawn(defaultUnitToSpawn);

        public void Spawn(Unit unitToSpawn)
        {
            if (cellsMoveToCellTaker.Take() == null || cellsMoveToCellTaker.Take().Count == 0)
                return;

            var spawnedUnit = Instantiate(unitToSpawn);
            spawnedUnit.currentCell = spawnerUnit.currentCell;
            spawnedUnit.transform.position = spawnedUnit.currentCell.transform.position;
            var selectedCell = cellsMoveToCellTaker.Take()[Random.Range(0, cellsMoveToCellTaker.Take().Count)];
            spawnedUnit.movable.Move(Map.Instance.GetCell(selectedCell.coordinates, spawnedUnit.unitType), 0,
                movementSpeed, jumpHeight, false);


            if (spawnedUnit.input != null)
                UnitsQueue.Instance.AddObjectInQueue(spawnedUnit);
        }
    }
}