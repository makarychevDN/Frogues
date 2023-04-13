using FroguesFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FroguesFramework
{
    public class IncreaseBlockOnContactWithOtherUnit : MonoBehaviour, IAbleToUseOnUnit
    {
        [SerializeField] private int blockValue;

        public bool PossibleToUseOnUnit(Unit target)
        {
            return true;
        }

        public void UseOnUnit(Unit target)
        {
            target.Health.IncreaseTemporaryArmor(blockValue);
        }

        public void Init(Unit unit)
        {
            unit.OnStepOnThisUnitByTheUnit.AddListener(UseOnUnit);
            unit.Movable.OnBumpIntoUnit.AddListener(UseOnUnit);
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

        public void VisualizePreUseOnUnit(Unit target)
        {
            throw new System.NotImplementedException();
        }
    }
}