
using UnityEngine.Events;

namespace FroguesFramework
{
    public interface IAbleToHaveCurrentAbility
    {
        public void SetCurrentAbility(IAbility ability);
        
        public IAbility GetCurrentAbility();

        public void ClearCurrentAbility();
    }
}

