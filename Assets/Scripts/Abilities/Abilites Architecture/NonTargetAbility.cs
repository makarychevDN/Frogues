namespace FroguesFramework
{
    public abstract class NonTargetAbility : AbleToUseAbility, IAbleToUseWithNoTarget
    {
        public virtual bool PossibleToUse() => _owner.ActionPoints.IsPointsEnough(cost);
        public abstract void Use();
    }
}