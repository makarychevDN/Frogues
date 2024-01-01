using UnityEngine;

using System.Collections.Generic;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class SelectUnitToCellAbility : UnitTargetAbility, IAbleToReturnIsPrevisualized, IAbleToDealDamage
    {
        [SerializeField] protected DamageType damageType;
        [SerializeField] protected int damage;
        [SerializeField] private int radius;
        [SerializeField] private AreaTargetAbility areaTargetAbility;
        [SerializeField] private bool isAbleToSelectDefaultUnit = true;
        [SerializeField] private bool isAbleToSelectBloodSurface = false;

        [Header("Previsualization Setup")]
        [SerializeField] protected LineRenderer lineFromOwnerToTarget;

        private bool _isPrevisualizedNow;
        private Unit _hashedTarget;

        public UnityEvent<Unit> OnUnitSelected;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            areaTargetAbility.Init(unit);
            OnUnitSelected.AddListener(HashUnitToAreaTargetAbility);
        }

        private void HashUnitToAreaTargetAbility(Unit unit)
        {
            ((IAbleToHashUnitTarget)areaTargetAbility).HashUnitTarget(unit);
        }

        public override int CalculateHashFunctionOfPrevisualisation()
        {
            int value = _usingArea.Count;

            if (_hashedTarget != null)
                value ^= _hashedTarget.Coordinates.x ^ _hashedTarget.Coordinates.y ^ _hashedTarget.gameObject.name.GetHashCode();

            return value ^ GetHashCode();
        }

        public override List<Cell> CalculateUsingArea() => _usingArea = CellsTaker.TakeCellsAreaByRange(_owner.CurrentCell, radius);

        public override void DisablePreVisualization()
        {
            lineFromOwnerToTarget.gameObject.SetActive(false);
            _isPrevisualizedNow = false;
        }

        public bool IsPrevisualizedNow()
        {
            return _isPrevisualizedNow || ((IAbleToReturnIsPrevisualized)areaTargetAbility).IsPrevisualizedNow();
        }

        public override bool PossibleToUseOnUnit(Unit target)
        {
            if (target == null)
                return false;

            return IsResoursePointsEnough() && _usingArea.Contains(target.CurrentCell);
        }

        public override void PrepareToUsing(Unit target)
        {
            _hashedTarget = target;
            CalculateUsingArea();
        }

        public override void UseOnUnit(Unit target)
        {
            if (target == null)
                return;

            OnUnitSelected.Invoke(target);
            _owner.AbilitiesManager.AbleToHaveCurrentAbility.SetCurrentAbility(areaTargetAbility);
        }

        public override void VisualizePreUseOnUnit(Unit target)
        {
            _isPrevisualizedNow = true;
            _usingArea.ForEach(cell => cell.EnableValidForAbilityCellHighlight(_usingArea));

            if (target != null)
                target.MaterialInstanceContainer.EnableOutline(true);

            if (!PossibleToUseOnUnit(target))
                return;

            target.Health.PreTakeDamage(CalculateDamage());
            _owner.ActionPoints.PreSpendPoints(actionPointsCost);
            _owner.BloodPoints.PreSpendPoints(bloodPointsCost);
            lineFromOwnerToTarget.gameObject.SetActive(true);
            lineFromOwnerToTarget.SetPosition(0, _owner.SpriteParent.position);
            lineFromOwnerToTarget.SetPosition(1, target.SpriteParent.position);
        }

        public override bool CheckItUsableOnDefaultUnit() => isAbleToSelectDefaultUnit;

        public override bool CheckItUsableOnBloodSurfaceUnit() => isAbleToSelectBloodSurface;

        public virtual int GetDefaultDamage() => damage;

        public virtual DamageType GetDamageType() => damageType;

        public virtual int CalculateDamage() => Extensions.CalculateDamageWithGameRules(damage, damageType, _owner.Stats);
    }
}