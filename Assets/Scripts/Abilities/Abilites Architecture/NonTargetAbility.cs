namespace FroguesFramework
{
    public abstract class NonTargetAbility : AbleToCostAbility, IAbleToUseWithNoTarget
    {
        public virtual void IsUsePossible() => _owner.ActionPoints.IsActionPointsEnough(cost);
        public abstract void Use();
    }
}