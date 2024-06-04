using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public abstract class AnyTargetAbility : AbleToUseAbility, IAbleToCalculateUsingArea, IAbleToDisablePreVisualization, IAbleToCalculateHashFunctionOfPrevisualisation
    {
        [SerializeField] protected bool needToRotateOwnersSprite = true;

        protected List<Cell> _usingArea;
        public abstract List<Cell> CalculateUsingArea();
        public abstract void DisablePreVisualization();

        public void AddSelfToTheList() => _owner.CurrentRoom.AddAbleToDisablePrevisualizationObject(this);
        public void RemoveSelfFromTheList() => _owner.CurrentRoom.RemoveAbleToDisablePrevisualizationObject(this);

        public override void Init(Unit unit)
        {
            base.Init(unit);
            AddSelfToTheList();
        }

        private void OnDestroy()
        {
            if(_owner != null)
                RemoveSelfFromTheList();
        }

        public abstract int CalculateHashFunctionOfPrevisualisation();
    }
}