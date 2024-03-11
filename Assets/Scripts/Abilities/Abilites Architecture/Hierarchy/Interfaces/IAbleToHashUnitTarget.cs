using UnityEngine.Events;

namespace FroguesFramework
{
    public interface IAbleToHashUnitTarget
    {
        public void HashUnitTargetAndCosts(Unit target, int actionPointsCost, int bloodPointsCost, int damage);
        public UnityEvent GetOnUseEvent();
    }
}