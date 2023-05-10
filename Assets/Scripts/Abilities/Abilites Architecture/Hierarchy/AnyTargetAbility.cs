using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public abstract class AnyTargetAbility : AbleToUseAbility, IAbleToCalculateUsingArea, IAbleToDisablePreVisualization
    {
        [SerializeField] protected bool needToRotateOwnersSprite = true;

        protected List<Cell> _usingArea;
        public abstract List<Cell> CalculateUsingArea();
        public abstract void DisablePreVisualization();

        public void AddMySelfToEntryPoint() => 
            EntryPoint.Instance.AddAbleToDisablePreVisualizationToCollection(this);

        public void RemoveMySelfFromEntryPoint() => 
            EntryPoint.Instance.RemoveAbleToDisablePreVisualizationToCollection(this);

        public override void Init(Unit unit)
        {
            base.Init(unit);
            AddMySelfToEntryPoint();
        }

        private void OnDestroy()
        {
            RemoveMySelfFromEntryPoint();
        }
    }
}