using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    [RequireComponent(typeof(Unit))]
    public class SurfaceUnitExtension : MonoBehaviour
    {
        public UnityEvent OnStepFromThisUnit = new UnityEvent();
        public UnityEvent<Unit> OnStepFromThisUnitByTheUnit = new UnityEvent<Unit>();

        private Unit _unit;


        public void Init(Unit unit)
        {
            _unit = unit;

            unit.CurrentCell.OnBecameFull.AddListener(() => unit.OnStepOnThisUnit.Invoke());
            unit.CurrentCell.OnBecameFullByUnit.AddListener(InvokeOnStepOnThisUnitByTheUnit);

            unit.CurrentCell.OnBecameEmpty.AddListener(() => OnStepFromThisUnit.Invoke());
            unit.CurrentCell.OnBecameFullByUnit.AddListener(InvokeOnStepFromThisUnitByTheUnit);
        }

        private void InvokeOnStepOnThisUnitByTheUnit(Unit unit) => unit.OnStepOnThisUnitByUnit.Invoke(unit);
        private void InvokeOnStepFromThisUnitByTheUnit(Unit unit) => OnStepFromThisUnitByTheUnit.Invoke(unit);
    }
}