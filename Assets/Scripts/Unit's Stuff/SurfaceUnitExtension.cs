using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    [RequireComponent(typeof(Unit))]
    public class SurfaceUnitExtension : MonoBehaviour
    {
        public UnityEvent OnStepFromThisUnit = new UnityEvent();
        public UnityEvent<Unit> OnStepFromThisUnitByTheUnit = new UnityEvent<Unit>();
        public UnityEvent OnStepOnThisUnit => unit.OnStepOnThisUnit;
        public UnityEvent<Unit> OnStepOnThisUnitByUnit => unit.OnStepOnThisUnitByUnit;

        private Unit unit;


        private void Start()
        {
            unit = GetComponent<Unit>();

            unit.CurrentCell.OnBecameFull.AddListener(() => OnStepOnThisUnit.Invoke());
            unit.CurrentCell.OnBecameFullByUnit.AddListener(InvokeOnStepOnThisUnitByTheUnit);

            unit.CurrentCell.OnBecameEmpty.AddListener(() => OnStepFromThisUnit.Invoke());
            unit.CurrentCell.OnBecameFullByUnit.AddListener(InvokeOnStepFromThisUnitByTheUnit);
        }

        private void InvokeOnStepOnThisUnitByTheUnit(Unit unit) => OnStepOnThisUnitByUnit.Invoke(unit);
        private void InvokeOnStepFromThisUnitByTheUnit(Unit unit) => OnStepFromThisUnitByTheUnit.Invoke(unit);
    }
}