
namespace FroguesFramework
{
    public interface IAbleToHaveCurrentAbility
    {
        public void SetCurrentAbility(IAbility ability);

        public void ClearCurrentAbility();
    }
}

