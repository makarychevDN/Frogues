using System.Collections.Generic;
using UnityEngine;
using static FroguesFramework.RewardsGenerator;

namespace FroguesFramework
{
    [CreateAssetMenu(fileName = "Ascension Setup", menuName = "ScriptableObjects/Ascension Setup", order = 1)]
    public class AscensionSetup : ScriptableObject
    {
        [field: SerializeField] public int index { get; private set; }
        [field: SerializeField] public int RequaredDeltaOfScoreToOpenExitToCampfire { get; private set; }
        [field: SerializeField] public int ExpectedMinimumOfEnemiesOnMap { get; private set; }
        [field: SerializeField] public int WavesOfEnemiesRequariedToSpawnRookashka { get; private set; }
        [field: SerializeField] public List<PoolOfEnemiesDifficultySetup> DifficultyOfEnemiesInThePoolSetup { get; private set; }
        [field: SerializeField] public List<RewardPanelSetup> Rewards { get; private set; }
        [field: SerializeField] public bool someEnemiesGetAdditionalMaxOfActionPoints { get; private set; }
        [field: SerializeField] public bool someRangeEnemiesAreAbleToRetreat { get; private set; }
    }
}