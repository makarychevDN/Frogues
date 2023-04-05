using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class WaveSpawner : MonoBehaviour, IRoundTickable
    {
        [SerializeField] private List<WavesSetup> waves;
        [SerializeField] private int _roundsCounter;
        
        public void TickBeforePlayerTurn()
        {
            _roundsCounter++;

            var currentWave = waves.FirstOrDefault(wave => wave.RoundsToSpawn == _roundsCounter);
            
            if(currentWave == null)
                return;
            
            SpawnWave(currentWave);
            waves.Remove(currentWave);
        }

        public void TickBeforeEnemiesTurn()
        {
            //do nothing
        }

        public void SpawnWave(WavesSetup wave)
        {
            var emptyCells = CellsTaker.TakeAllEmptyCells();

            for (int i = 0; i < wave.UnitsAndSpawnChances.Count; i++)
            {
                if(emptyCells.Count == 0)
                    break;
                
                var cellToSpawn = emptyCells.GetRandomElement();
                emptyCells.Remove(cellToSpawn);
                //SpawnAndMoveToCell(cellToSpawn, wave.UnitsAndSpawnChances[i]);
            }
        }
        
        public void SpawnAndMoveToCell(Cell targetCell, Unit prefabToSpawn)
        {
            var spawnedObject = Instantiate(prefabToSpawn, EntryPoint.Instance.CenterOfRoom + Vector3.up * 7, Quaternion.identity);
            spawnedObject.Init();
            spawnedObject.Movable.FreeCostMove(targetCell, 20, 1, false);
            EntryPoint.Instance.UnitsQueue.AddObjectInQueue(spawnedObject);
        }
    }
}