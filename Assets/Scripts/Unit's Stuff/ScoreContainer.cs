using UnityEngine;

namespace FroguesFramework
{
    public class ScoreContainer : MonoBehaviour
    {
        [SerializeField] private int score;

        public void Init(Unit unit)
        {
            unit.AbleToDie.OnOwnerWasKilled.AddListener(IncreaseScore);
        }

        private void IncreaseScore() => EntryPoint.Instance.IncreaseScore(score);
    }
}