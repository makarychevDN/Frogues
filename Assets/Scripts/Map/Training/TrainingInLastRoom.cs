using UnityEngine;
using UnityEngine.SceneManagement;

namespace FroguesFramework
{
    public class TrainingInLastRoom : BaseTrainingModificator
    {
        [SerializeField] private Vector2Int targetToStepCellCoordinates;
        private Cell _targetCell;

        public override void Init()
        {
            _targetCell = EntryPoint.Instance.Map.GetCell(targetToStepCellCoordinates);
            _targetCell.OnBecameFull.AddListener(GoToMainMenu);
        }

        private void GoToMainMenu()
        {
            SceneManager.LoadScene("main menu");
        }
    }
}