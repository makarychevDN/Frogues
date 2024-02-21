using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class WavesGenerator : MonoBehaviour, IRoundTickable
    {
        [SerializeField] private int expectedMinimumOfEnemiesOnTheMap = 3;
        [SerializeField] private int roundsToSpawn = 2;
        [SerializeField] private int roundsCounter;
        [SerializeField] private List<Unit> unitsPool;
        [SerializeField] private List<PossibleToSpawnEnemyProviderDueDifficultyLevel> commonSchemeOfPool;
        [SerializeField] private List<PoolOfEnemiesDifficultySetup> difficultyOfEnemiesInThePoolSetup;
        private PoolOfEnemiesDifficultySetup _currentDifficultySetup;
        private List<int> _currentDifficultySetupShuffeledValues;

        public PoolOfEnemiesDifficultySetup CalculateCurrentEnemyGeneratorSetup() => difficultyOfEnemiesInThePoolSetup.Last(waveSetup => EntryPoint.Instance.Score >= waveSetup.ScoreRequirements);

        public void SpawnEnemies()
        {
            int needToSpawnEnemies = expectedMinimumOfEnemiesOnTheMap - CellsTaker.TakeAllUnits().Where(unit => unit.IsEnemy && !unit.IsSummoned).Count();
            if (needToSpawnEnemies < 1) 
                needToSpawnEnemies = 1;

            for(int i = 0; i < needToSpawnEnemies; i++)
            {
                if (unitsPool.Count == 0)
                    GenerateAndAddPackOfEnemiesInThePool();

                SpawnEnemy();
            }

        }

        public void SpawnEnemy()
        {
            var emptyCells = CellsTaker.TakeAllEmptyCells();

            if (emptyCells.Count == 0)
                return;

            var cellToSpawn = emptyCells.GetRandomElement();
            var unitToSpawn = unitsPool.GetRandomElement();
            unitsPool.Remove(unitToSpawn);
            SpawnAndMoveToCell(cellToSpawn, unitToSpawn);
        }

        public void SpawnAndMoveToCell(Cell targetCell, Unit prefabToSpawn)
        {
            var spawnedObject = Instantiate(prefabToSpawn, EntryPoint.Instance.CenterOfRoom + Vector3.up * 7, Quaternion.identity);
            spawnedObject.Init();
            spawnedObject.Movable.Move(targetCell, 20, 1, false, true, false);
            EntryPoint.Instance.UnitsQueue.AddObjectInQueue(spawnedObject);
            spawnedObject.transform.parent = EntryPoint.Instance.Map.transform;
        }

        public void GenerateAndAddPackOfEnemiesInThePool()
        {
            if(CalculateCurrentEnemyGeneratorSetup() != null)
                _currentDifficultySetupShuffeledValues = new List<int>(CalculateCurrentEnemyGeneratorSetup().DifficultyOfEnemies);

            _currentDifficultySetupShuffeledValues.Shuffle();

            for(int i = 0; i < commonSchemeOfPool.Count; i++)
            {
                unitsPool.Add(commonSchemeOfPool[i].GetUnitByDifficultyLevel(_currentDifficultySetupShuffeledValues[i]));
            }
        }

        public void TickAfterPlayerTurn()
        {
            //do nothing
        }

        public void TickAfterEnemiesTurn()
        {
            if (EntryPoint.Instance.CurrentRoomIsHub || EntryPoint.Instance.ExitActivated)
                return;

            roundsCounter++;

            if (roundsCounter < roundsToSpawn)
                return;

            SpawnEnemies();
            ResetRoundsTimer();
        }

        public void ResetRoundsTimer() => roundsCounter = 0;
    }
}