namespace FroguesFramework
{
    public interface IAbleToHaveCurrentAbility
    {
        public void SetCurrentAbility(BaseAbility ability);
        
        public BaseAbility GetCurrentAbility();

        public void ClearCurrentAbility();
    }
}

