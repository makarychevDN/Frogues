namespace FroguesFramework
{
    public abstract class NonTargetAbility : AbleToUseAbility, IAbleToUseWithNoTarget
    {
        public virtual bool IsUsePossible() => _owner.ActionPoints.IsActionPointsEnough(cost);
        public abstract void Use();
    }
}