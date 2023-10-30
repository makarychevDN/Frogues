namespace FroguesFramework
{
    public abstract class UnitTargetAbility : AnyTargetAbility, IAbleToUseOnUnit
    {
        public abstract bool PossibleToUseOnUnit(Unit target);
        public abstract void PrepareToUsing(Unit target);
        public abstract void UseOnUnit(Unit target);
        public abstract void VisualizePreUseOnUnit(Unit target);
        public abstract bool CheckItUsableOnDefaultUnit();
        public abstract bool CheckItUsableOnBloodSurfaceUnit();
    }
}
