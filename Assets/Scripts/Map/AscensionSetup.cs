using UnityEngine;

namespace FroguesFramework
{
    [CreateAssetMenu(fileName = "Ascension Setup", menuName = "ScriptableObjects/Ascension Setup", order = 1)]
    public class AscensionSetup : ScriptableObject
    {
        [SerializeField] private int requaredDeltaOfScoreToOpenExitToCampfire;
        [SerializeField] private int expectedMinimumOfEnemiesOnMap;
        [SerializeField] private int wavesOfEnemiesRequariedToSpawnRookashka;
    }
}