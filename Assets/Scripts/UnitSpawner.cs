using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private List<Unit> unitsToSpawn;
    [SerializeField] private BaseCellsTaker spawnCellsTaker;

    public void Spawn()
    {
        if (spawnCellsTaker.Take() == null || spawnCellsTaker.Take().Count == 0)
            return;

        var spawnedUnit = Instantiate(unitsToSpawn[Random.Range(0, unitsToSpawn.Count)]);
        var selectedCell = spawnCellsTaker.Take()[Random.Range(0, spawnCellsTaker.Take().Count)];
        selectedCell = Map.Instance.GetLayerByType(spawnedUnit.unitType)[selectedCell.coordinates.x, selectedCell.coordinates.y];
        selectedCell.Content = spawnedUnit;
        spawnedUnit.transform.position = selectedCell.transform.position;

        if (spawnedUnit.input != null)
            UnitsQueue.Instance.AddObjectInQueue(spawnedUnit);
    }
}
