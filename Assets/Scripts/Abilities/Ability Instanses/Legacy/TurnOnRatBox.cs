using FroguesFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class TurnOnRatBox : MonoBehaviour
    {
        [SerializeField] private int armorValue;

        public int GetCost() => 0;

        public void Init(Unit unit)
        {
            unit.Health.IncreasePermanentArmor(armorValue);
            unit.Animator.SetBool("BoxIsOn", true);
            unit.Animator.SetTrigger(CharacterAnimatorParameters.ChangeWeapon);
            unit.Health.OnBlockDestroyed.AddListener(() => unit.Animator.SetBool("BoxIsOn", false));
        }

        public bool IsPartOfWeapon() => false;

        public void Use()
        {
            throw new System.NotImplementedException();
        }

        public void VisualizePreUse()
        {
            throw new System.NotImplementedException();
        }
    }
}