using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class WaveSpawner : MonoBehaviour, IRoundTickable
    {
        [SerializeField] private List<Wave> waves;
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

        public void SpawnWave(Wave wave)
        {
            var emptyCells = CellsTaker.TakeAllEmptyCells();

            for (int i = 0; i < wave.Units.Count; i++)
            {
                if(emptyCells.Count == 0)
                    break;
                
                var cellToSpawn = emptyCells.GetRandomElement();
                emptyCells.Remove(cellToSpawn);
                SpawnAndMoveToCell(cellToSpawn, wave.Units[i]);
            }
        }
        
        public void SpawnAndMoveToCell(Cell targetCell, Unit prefabToSpawn)
        {
            var spawnedObject = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
            spawnedObject.Init();
            spawnedObject.Movable.FreeCostMove(targetCell, 15, 1, false);
            EntryPoint.Instance.UnitsQueue.AddObjectInQueue(spawnedObject);
        }
        
        
        [Serializable]
        public class Wave
        {
            [field : SerializeField] public int RoundsToSpawn { get; private set; }
            [field : SerializeField] public List<Unit> Units { get; private set; }
        }
    }
}