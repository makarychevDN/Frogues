using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class WaveSpawner : MonoBehaviour, IRoundTickable
    {
        [SerializeField] private List<WavesSetup> waveSetups;
        [SerializeField] private WavesSetup currentWaveSetup;
        [SerializeField] private int _roundsCounter;
        
        public void ResetRoundsTimer() => _roundsCounter = 0;

        public void TickAfterEnemiesTurn()
        {
            if (EntryPoint.Instance.CurrentRoomIsPeaceful || EntryPoint.Instance.ExitActivated)
                return;

            _roundsCounter++;
            CalculateCurrentWaveSetup();

            if (_roundsCounter != currentWaveSetup.RoundsToSpawn)
                return;
                
            SpawnWave(currentWaveSetup);
            ResetRoundsTimer();
        }

        public void CalculateCurrentWaveSetup() => currentWaveSetup = waveSetups.Last(waveSetup => EntryPoint.Instance.Score >= waveSetup.ScoreRequirements);

        public void SpawnPreWave()
        {
            CalculateCurrentWaveSetup();
            var emptyCells = CellsTaker.TakeAllEmptyCells();

            foreach (var unit in currentWaveSetup.AdditionalUnitsOnStartOfMap)
            {
                if (emptyCells.Count == 0)
                    break;

                var cellToSpawn = emptyCells.GetRandomElement();
                emptyCells.Remove(cellToSpawn);
                SpawnAndMoveToCell(cellToSpawn, unit);
            }
        }

        public void TickAfterPlayerTurn()
        {
            //do nothing
        }

        public void SpawnWave(WavesSetup waveSetup)
        {
            var emptyCells = CellsTaker.TakeAllEmptyCells();

            for (int i = 0; i < waveSetup.SpawnedUnitsQuantityForWave; i++)
            {
                if(emptyCells.Count == 0)
                    break;
                
                var cellToSpawn = emptyCells.GetRandomElement();
                emptyCells.Remove(cellToSpawn);
                SpawnAndMoveToCell(cellToSpawn, waveSetup.GetRandomUnit());
            }
        }
        
        public void SpawnAndMoveToCell(Cell targetCell, Unit prefabToSpawn)
        {
            var spawnedObject = Instantiate(prefabToSpawn, EntryPoint.Instance.CenterOfRoom + Vector3.up * 7, Quaternion.identity);
            spawnedObject.Init();
            spawnedObject.Movable.Move(targetCell, 20, 1, false, true, false);
            EntryPoint.Instance.UnitsQueue.AddObjectInQueue(spawnedObject);
            spawnedObject.transform.parent = EntryPoint.Instance.Map.transform;
        }
    }
}