namespace FroguesFramework
{
    public abstract class NonTargetAbility : AbleToUseAbility, IAbleToUseWithNoTarget
    {
        public virtual bool PossibleToUse() => _owner.ActionPoints.IsPointsEnough(bloodPointsCost);
        public abstract void Use();
    }
}