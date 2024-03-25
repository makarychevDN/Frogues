using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class TrainingInFourthRoom : BaseTrainingModificator
    {
        [SerializeField] private List<GameObject> enableAfterStepInTargetCell;
        [SerializeField] private Vector2Int targetToStepCellCoordinates;
        private Cell _targetToStepCell;

        public override void Init()
        {
            _targetToStepCell = EntryPoint.Instance.Map.GetCell(targetToStepCellCoordinates);
            enableAfterStepInTargetCell.ForEach(go => go.SetActive(false));
            _targetToStepCell.OnBecameFull.AddListener(EnableObjects);
        }

        private void EnableObjects()
        {
            enableAfterStepInTargetCell.ForEach(go => go.SetActive(true));
            _targetToStepCell.OnBecameFull.RemoveListener(EnableObjects);
        }
    }
}