using System;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    [CreateAssetMenu(fileName = "Enemies Provider", menuName = "ScriptableObjects/Enemies Provider Due Difficulty Level", order = 1)]
    public class PossibleToSpawnEnemyProviderDueDifficultyLevel : ScriptableObject
    {
        [SerializeField] private List<PossibleSpawnVariationsWithTheSameDefficultyLevel> setup;
        private Dictionary<int, List<Unit>> unitsByDifficultyDictionary = new Dictionary<int, List<Unit>>();

        private void Init()
        {
            unitsByDifficultyDictionary.Clear();
            foreach (var setupInstance in setup)
            {
                unitsByDifficultyDictionary.Add(setupInstance.difficultyLevel, setupInstance.units);
                Debug.Log(setupInstance.difficultyLevel);
            }
        }

        public Unit GetUnitByDifficultyLevel(int difficultyLevel)
        {
            if()

            return unitsByDifficultyDictionary[difficultyLevel].GetRandomElement();
        }

        [Serializable]
        public struct PossibleSpawnVariationsWithTheSameDefficultyLevel
        {
            public int difficultyLevel;
            public List<Unit> units;
        }
    }
}