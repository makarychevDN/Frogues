namespace FroguesFramework
{
    public class SelectUnitToBackStab : SelectUnitToCellAbility
    {
        public override void VisualizePreUseOnUnit(Unit target)
        {
            base.VisualizePreUseOnUnit(target);

            if(target != null)
                target.Health.PreTakeDamage(_owner.AbilitiesManager.WeaponDamage);
        }

        public override int GetDefaultDamage() => _owner.AbilitiesManager.WeaponDamage;

        public override DamageType GetDamageType() => damageType;

        public override int CalculateDamage() => Extensions.CalculateDamageWithGameRules(_owner.AbilitiesManager.WeaponDamage, damageType, _owner.Stats);

        public override int GetActionPointsCost() => CalculateActionPointsCost;

        protected override int CalculateActionPointsCost => _owner.AbilitiesManager.WeaponActionPointsCost == 2 ? 3 : 1;
    }
}