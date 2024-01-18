using UnityEngine;

namespace FroguesFramework
{
    public class DealDamageToClosestEnemiesOnBlockOrArmorIncreased : PassiveAbility, IAbleToReturnSingleValue
    {
        [SerializeField] private int damageValue;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.Health.OnBlockIncreased.AddListener(DealDamageToClosestEnemies);
        }

        private void DealDamageToClosestEnemies()
        {
            foreach(var cell in _owner.CurrentCell.CellNeighbours.GetAllNeighbors())
            {
                if(!cell.IsEmpty && cell.Content is not Barrier)
                {
                    cell.Content.Health.TakeDamage(damageValue, null);
                }
            }
        }

        public override void UnInit()
        {
            base.UnInit();
            _owner.Health.OnBlockIncreased.RemoveListener(DealDamageToClosestEnemies);
        }

        public int GetValue() => damageValue;
    }
}