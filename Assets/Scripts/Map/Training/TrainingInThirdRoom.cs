
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class TrainingInThirdRoom : BaseTrainingModificator
    {
        [SerializeField] private List<GameObject> objectsToEnableAfterInspectTargetUnit;
        [SerializeField] private Unit targetToInspectUnit;
        [SerializeField] private InspectAbility inspectAbility;

        public override void Init()
        {
            EntryPoint.Instance.MetaPlayer.MovementAbility.IncreaseActionPointsCost(1);
            EntryPoint.Instance.MetaPlayer.GetComponentsInChildren<Collider>().ToList().ForEach(collider => collider.enabled = true);
            inspectAbility.Init(EntryPoint.Instance.MetaPlayer);

            objectsToEnableAfterInspectTargetUnit.ForEach(go => go.SetActive(false));
            targetToInspectUnit.OnInspectIt.AddListener(EnableObjects);
        }

        private void EnableObjects()
        {
            objectsToEnableAfterInspectTargetUnit.ForEach(go => go.SetActive(true));
            targetToInspectUnit.OnInspectIt.RemoveListener(EnableObjects);
            EntryPoint.Instance.EndTurnButton.SetActive(true);
        }
    }
}