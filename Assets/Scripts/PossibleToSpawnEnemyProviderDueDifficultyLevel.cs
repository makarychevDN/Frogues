using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    [CreateAssetMenu(fileName = "Enemies Provider", menuName = "ScriptableObjects/Enemies Provider Due Difficulty Level", order = 1)]
    public class PossibleToSpawnEnemyProviderDueDifficultyLevel : ScriptableObject
    {
        [SerializedDictionary("difficulty level", "due enemies")]
        [SerializeField] private SerializedDictionary<int, List<Unit>> unitsByDifficultyDictionary = new SerializedDictionary<int, List<Unit>>();

        public Unit GetUnitByDifficultyLevel(int difficultyLevel)
        {
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