using System;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class TrainingInSecondRoom : BaseTrainingModificator
    {
        [SerializeField] private List<GameObject> enableAfterInspectObjects;
        [SerializeField] private Unit targetToInspectUnit;
        
        public override void Init()
        {
            enableAfterInspectObjects.ForEach(go => go.SetActive(false));
            targetToInspectUnit.OnInspectIt.AddListener(EnableObjects);
        }

        private void EnableObjects()
        {
            enableAfterInspectObjects.ForEach(go => go.SetActive(true));
            targetToInspectUnit.OnInspectIt.RemoveListener(EnableObjects);

        }

    }
}