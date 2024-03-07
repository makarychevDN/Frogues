using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class TrainingInFourthRoom : BaseTrainingModificator
    {
        [SerializeField] private List<GameObject> enableAfterStepInTargetCell;
        [SerializeField] private Cell targetToStepCell;

        public override void Init()
        {
            enableAfterStepInTargetCell.ForEach(go => go.SetActive(false));
            targetToStepCell.OnBecameFull.AddListener(EnableObjects);
        }

        private void EnableObjects()
        {
            enableAfterStepInTargetCell.ForEach(go => go.SetActive(true));
            targetToStepCell.OnBecameFull.RemoveListener(EnableObjects);
        }
    }
}