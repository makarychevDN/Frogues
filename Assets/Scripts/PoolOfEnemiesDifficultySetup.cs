using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    [CreateAssetMenu(fileName = "Pool Of Enemies Difficulty Setup", menuName = "ScriptableObjects/Pool Of Enemies Difficulty Setup", order = 1)]
    public class PoolOfEnemiesDifficultySetup : ScriptableObject
    {
        [field: SerializeField] public int ScoreRequirements;
        [field: SerializeField] public List<int> DifficultyOfEnemies;
    }
}