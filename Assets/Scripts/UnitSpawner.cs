using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class UnitSpawner : MonoBehaviour
    {
        [SerializeField] private List<Unit> unitsToSpawn;
        [SerializeField] private BaseCellsTaker spawnCellsTaker;

        public void Spawn() => Spawn(1);
        
        public void Spawn(IntContainer intContainer) => Spawn(intContainer.Content);

        public void Spawn(int unitsQuantity)
        {
            if (spawnCellsTaker.Take() == null || spawnCellsTaker.Take().Count == 0)
                return;

            for (int i = 0; i < unitsQuantity; i++)
            {
                var spawnedUnit = Instantiate(unitsToSpawn[Random.Range(0, unitsToSpawn.Count)]);
                var selectedCell = spawnCellsTaker.Take()[Random.Range(0, spawnCellsTaker.Take().Count)];
                selectedCell =
                    Map.Instance.GetLayerByType(spawnedUnit.unitType)[selectedCell.coordinates.x,
                        selectedCell.coordinates.y];
                selectedCell.Content = spawnedUnit;
                spawnedUnit.transform.position = selectedCell.transform.position;

                if (spawnedUnit.input != null)
                    UnitsQueue.Instance.AddObjectInQueue(spawnedUnit);
            }
        }
    }
}