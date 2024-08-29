using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class IncreaseHpAndBloodAndAddStatEffects : PassiveAbility, IAbleToApplySpikesModificator
        , IAbleToModifyMaxHP, IAbleToModifyMaxBloodPoints
    {
        [SerializeField] private int additionalHp;
        [SerializeField] private int additionalMaxBlood;
        [SerializeField] private List<StatEffect> effects;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.Health.IncreaseMaxHp(additionalHp);

            if(additionalHp > 0)
                _owner.Health.TakeHealing(additionalHp);

            _owner.BloodPoints.IncreaseMaxPoints(additionalMaxBlood);
            effects.ForEach(effect => _owner.Stats.AddStatEffect(effect));
        }

        public override void UnInit()
        {
            _owner.Health.IncreaseMaxHp(-additionalHp);
            _owner.BloodPoints.IncreaseMaxPoints(-additionalMaxBlood);
            effects.ForEach(effect => _owner.Stats.RemoveStatEffect(effect));
            base.UnInit();
        }

        public int GetModificatorForMaxHP() => additionalHp;

        public int GetModificatorForMaxBloodPoints() => additionalMaxBlood;

        #region IAbleToApplySpikesModificator
        public int GetSpikesModificatorValue() => Extensions.GetModificatorValue(effects, StatEffectTypes.thorns);

        public int GetdeltaOfSpikesValueForEachTurn() => Extensions.GetDeltaValueOfModificatorForEachTurn(effects, StatEffectTypes.thorns);

        public int GetTimeToEndOfSpikesEffect() => Extensions.GetTimeToEndOfEffect(effects, StatEffectTypes.thorns);

        public bool GetSpikesEffectIsConstantly() => Extensions.GetEffectIsConstantly(effects, StatEffectTypes.thorns);
        #endregion
    }
}   