using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class ScoreContainer : MonoBehaviour
    {
        [SerializeField] private int score;

        public void Init(Unit unit)
        {
            unit.AbleToDie.OnDeath.AddListener(IncreaseScore);
        }

        private void IncreaseScore() => EntryPoint.Instance.IncreaseScore(score);
    }
}