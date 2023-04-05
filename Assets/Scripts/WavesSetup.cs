using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    [CreateAssetMenu(fileName = "Waves Setup", menuName = "ScriptableObjects/WavesSetup", order = 1)]
    public class WavesSetup : ScriptableObject
    {
        [field: SerializeField] public int ScoreRequirements { get; private set; }
        [field: SerializeField] public int RoundsToSpawn { get; private set; }
        [field: SerializeField] public int SpawnedUnitsQuantityForWave { get; private set; }
        [field: SerializeField] public List<Unit> AdditionalUnitsOnStartOfMap { get; private set; }
        [field: SerializeField] public List<UnitAndSpawnChance> UnitsAndSpawnChances { get; private set; }
    }

    [Serializable]
    public class UnitAndSpawnChance
    {
        [field: SerializeField] public Unit Unit { get; private set; }
        [field: SerializeField] public float SpawnChance { get; private set; }
    }
}