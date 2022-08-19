using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class WavesSpawner : MonoBehaviour
    {
        [SerializeField] private List<WaveAndTimer> waves;
        [SerializeField] private RoundsCounter roundsCounter;
        [SerializeField] private SummonAndMove summonAndMove;

        public void TryToSpawnWave()
        {
            foreach (var waveAndTimer in waves)
            {
                if(waveAndTimer.roundsToSpawn != roundsCounter.Count)
                    continue;
                
                waveAndTimer.wave.Content.ForEach(unit => summonAndMove.Spawn(unit));
            }
        }
        
        [Serializable]
        private struct WaveAndTimer
        {
            public int roundsToSpawn;
            public WaveOfEnemies wave;
        }
    }
}