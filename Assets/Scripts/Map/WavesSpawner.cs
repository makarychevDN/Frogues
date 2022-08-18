using System;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class WavesSpawner : MonoBehaviour
    {
        [SerializeField] private List<WaveAndTimer> waves;
        
        [Serializable]
        private struct WaveAndTimer
        {
            public int roundsToSpawn;
            public WaveOfEnemies wave;
        }
    }
}