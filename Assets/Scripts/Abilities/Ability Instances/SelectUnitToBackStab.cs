namespace FroguesFramework
{
    public class SelectUnitToBackStab : SelectUnitToCellAbility
    {
        protected override int CalculateActionPointsCost => _owner.AbilitiesManager.WeaponActionPointsCost == 2 ? 1 : 0;

        public override void VisualizePreUseOnUnit(Unit target)
        {
            base.VisualizePreUseOnUnit(target);

            if(target != null)
                target.Health.PreTakeDamage(_owner.AbilitiesManager.WeaponDamage);
        }
    }
}