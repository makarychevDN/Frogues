using UnityEngine;

namespace FroguesFramework
{
    public class DealDamageToSteppedOnThisUnitPassiveAbility : MonoBehaviour, IAbleToUseOnTarget, IAbility
    {
        [SerializeField] private int damage;
        
        public bool PossibleToUseOnTarget(Unit target)
        {
            return true;
        }
        
        public void UseOnTarget(Unit target)
        {
            target.Health.TakeDamage(damage);
        }

        public void Init(Unit unit)
        {
            unit.OnStepOnThisUnitByTheUnit.AddListener(UseOnTarget);
            unit.Movable.OnBumpIntoUnit.AddListener(UseOnTarget);
        }
        
        //todo need to remake interfaces stuff to remove this

        public int GetCost()
        {
            throw new System.NotImplementedException();
        }

        public bool IsPartOfWeapon()
        {
            throw new System.NotImplementedException();
        }

        public void VisualizePreUse()
        {
            throw new System.NotImplementedException();
        }

        public void Use()
        {
            throw new System.NotImplementedException();
        }
    }
}