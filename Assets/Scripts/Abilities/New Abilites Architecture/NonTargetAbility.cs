namespace FroguesFramework
{
    public abstract class NonTargetAbility : AbleToCostAbility, IAbleToUseWithNoTarget
    {
        public abstract void IsUsePossible();
        public abstract void Use();
    }
}