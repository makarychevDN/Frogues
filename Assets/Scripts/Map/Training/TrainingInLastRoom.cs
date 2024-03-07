using UnityEngine;
using UnityEngine.SceneManagement;

namespace FroguesFramework
{
    public class TrainingInLastRoom : BaseTrainingModificator
    {
        [SerializeField] private Cell TargetCell;

        public override void Init()
        {
            TargetCell.OnBecameFull.AddListener(GoToMainMenu);
        }

        private void GoToMainMenu()
        {
            SceneManager.LoadScene("main menu");
        }
    }
}