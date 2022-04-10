using UnityEngine;

namespace FroguesFramework
{
    public class UnitsScoreContainer : IntContainer
    {
        [SerializeField] private AbleToDie ableToDie;

        private void Start()
        {
            ableToDie.OnDeath.AddListener(IncreaseScore);
        }

        private void IncreaseScore()
        {
            FindObjectOfType<Score>().Content += Content;
        }
    }
}