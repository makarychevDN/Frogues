namespace FroguesFramework
{
    public abstract class UnitTargetAbility : AnyTargetAbility, IAbleToUseOnUnit
    {
        public abstract bool PossibleToUseOnUnit(Unit target);
        public abstract void UseOnUnit(Unit target);
        public abstract void VisualizePreUseOnUnit(Unit target);
    }
}