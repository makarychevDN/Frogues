namespace FroguesFramework
{
    public interface IAbleToHaveNativeAttack
    {
        public void SetCurrentNativeAttack(IAbleToBeNativeAttack ableToBeNativeAttack);

        public UnitTargetAbility GetCurrentNativeAttack();
    }
}