using System;
using System.Collections;
using UnityEngine;

namespace FroguesFramework
{
    public class GenericSelfBuffAbility : NonTargetAbility
    {
        [Serializable]
        public struct BuffEffect
        {
            public StatEffectTypes BuffType;
            public int BuffValue;
            public int TimeToEndBuff;
            public bool BuffIsConstantly;
        }

        [Space, Header("Ability Settings")]
        [SerializeField] private int temporaryBlockValue;
        [SerializeField] private int permanentBlockValue;
        [SerializeField] private BuffEffect[] Buffs;

        public override void Use()
        {
            if (!PossibleToUse())
                return;

            SpendResourcePoints();
            SetCooldownAsAfterUse();

            CurrentlyActiveObjects.Add(this);
            _owner.Animator.SetTrigger(abilityAnimatorTrigger.ToString());
            StartCoroutine(ApplyEffect(timeBeforeImpact));
            Invoke(nameof(RemoveCurrentlyActive), fullAnimationTime);
        }

        protected virtual IEnumerator ApplyEffect(float time)
        {
            yield return new WaitForSeconds(time);
            
            if (temporaryBlockValue != 0)
                _owner.Health.IncreaseTemporaryBlock(temporaryBlockValue);
            if (permanentBlockValue != 0)
                _owner.Health.IncreasePermanentBlock(permanentBlockValue);
            
            foreach (BuffEffect buff in Buffs)
                _owner.Stats.AddStatEffect(new StatEffect(buff.BuffType, buff.BuffValue, buff.TimeToEndBuff,
                    effectIsConstantly: buff.BuffIsConstantly));
        }

        private void RemoveCurrentlyActive() => CurrentlyActiveObjects.Remove(this);
    }
}