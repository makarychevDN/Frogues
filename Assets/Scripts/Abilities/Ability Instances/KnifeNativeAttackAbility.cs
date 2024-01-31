using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class KnifeNativeAttackAbility : DefaultCritableUnitTargetAbility
    {
        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.AbleToSkipTurn.OnSkipTurn.AddListener(TurnOffCriticalMode);
            _owner.Movable.OnMovementEnd.AddListener(TurnOnCriticalMode);
            OnEffectApplied.AddListener(TurnOffCriticalMode);
        }

        public override void UnInit()
        {
            _owner.AbleToSkipTurn.OnSkipTurn.RemoveListener(TurnOffCriticalMode);
            _owner.Movable.OnMovementEnd.RemoveListener(TurnOnCriticalMode);
            OnEffectApplied.RemoveListener(TurnOffCriticalMode);
            base.UnInit();
        }       
    }
}