using UnityEngine;

namespace FroguesFramework
{
    public class IncreaseHpAndBloodAndAddStatEffects : PassiveAbility
        , IAbleToModifyMaxHP, IAbleToModifyMaxBloodPoints
    {
        [SerializeField] private int additionalHp;
        [SerializeField] private int additionalMaxBlood;

        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.Health.IncreaseMaxHp(additionalHp);

            if(additionalHp > 0)
                _owner.Health.TakeHealing(additionalHp);

            _owner.BloodPoints.IncreaseMaxPoints(additionalMaxBlood);
        }

        public override void UnInit()
        {
            _owner.Health.IncreaseMaxHp(-additionalHp);
            _owner.BloodPoints.IncreaseMaxPoints(-additionalMaxBlood);
            base.UnInit();
        }

        public int GetModificatorForMaxHP() => additionalHp;

        public int GetModificatorForMaxBloodPoints() => additionalMaxBlood;
    }
}   