using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class HeavySteps : PassiveAbility, IAbleToReturnRange, IAbleToDealDamage, IAbleToHaveDelta
    {
        [SerializeField] private int damagePerMaxHpStep;
        [SerializeField] private int maxHpDeltaStep;
        [SerializeField] private int radius = 1;
        [SerializeField] private GameObject visualizationEffect;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.Movable.OnMovementEnd.AddListener(MakeShockWave);
            visualizationEffect.transform.parent = _owner.transform;
            visualizationEffect.transform.localPosition = Vector3.zero + Vector3.up * 0.05f;
        }

        public override void UnInit()
        {
            _owner.Movable.OnMovementEnd.RemoveListener(MakeShockWave);
            base.UnInit();
        }

        private void MakeShockWave()
        {
            List<Cell> targetsCells = EntryPoint.Instance.PathFinder.GetCellsAreaForAOE(_owner.CurrentCell, radius, true, false);
            targetsCells.Where(cell => cell.Content != null).ToList().ForEach(cell => cell.Content.Health.TakeDamage(CalculateDamage(), null));
            visualizationEffect.SetActive(true);
            Invoke(nameof(TurnOffVisualizationEffect), 0.1f);
        }

        private void TurnOffVisualizationEffect() => visualizationEffect.SetActive(false);

        public int CalculateDamage() => _owner.Health.MaxHp / maxHpDeltaStep * damagePerMaxHpStep;

        public int ReturnRange() => radius;

        public int GetDefaultDamage() => CalculateDamage();

        public DamageType GetDamageType() => DamageType.none;

        public int GetDeltaValue() => damagePerMaxHpStep;

        public int GetStepValue() => maxHpDeltaStep;
    }
}