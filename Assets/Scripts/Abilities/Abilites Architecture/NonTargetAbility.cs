namespace FroguesFramework
{
    public abstract class NonTargetAbility : AbleToUseAbility, IAbleToUseWithNoTarget
    {
        public virtual bool PossibleToUse() => _owner.ActionPoints.IsPointsEnough(actionPointsCost) && _owner.BloodPoints.IsPointsEnough(bloodPointsCost);
        public abstract void Use();
    }
}