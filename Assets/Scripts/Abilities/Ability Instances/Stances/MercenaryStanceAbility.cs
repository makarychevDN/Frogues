using UnityEngine;

namespace FroguesFramework
{
    public class MercenaryStanceAbility: BattleStanceAbility, IAbleToApplyStrenghtModificator, IAbleToApplyIntelligenceModificator
    {
        [Header("Odd turn")]
        [SerializeField] private int strenghtBuff;
        [SerializeField] private int intelegenceDebuff;
        [Header("Even turn")]
        [SerializeField] private int strenghtDebuff;
        [SerializeField] private int intelegenceBuff;

        private StatEffect _strenghtEffect = new (StatEffectTypes.strenght, 0, 1, effectIsConstantly: true);
        private StatEffect _intelegenceEffect = new (StatEffectTypes.intelegence, 0, 1, effectIsConstantly: true);
        
        private bool _isActive = false;
        private bool _isOddTurn = true;
        
        public override void Init(Unit unit)
        {
            base.Init(unit);

            _owner.Stats.AddStatEffect(_strenghtEffect);
            _owner.Stats.AddStatEffect(_intelegenceEffect);          
        }
        
        public override void ApplyEffect(bool isActive)
        {
            base.ApplyEffect(isActive);

            _isActive = isActive;
            if (isActive == false)
            {
                _isOddTurn = true;
                _strenghtEffect.Value = 0;
                _intelegenceEffect.Value = 0;
            }
        }

        public override void TickAfterPlayerTurn()
        {
            base.TickAfterPlayerTurn();

            if (!_isActive || !_owner.IsEnemy)
                return;
            _strenghtEffect.Value = _isOddTurn ? strenghtBuff : strenghtDebuff;
            _intelegenceEffect.Value = _isOddTurn ? intelegenceDebuff : intelegenceBuff;
            _isOddTurn = !_isOddTurn;
        }

        public override void TickAfterEnemiesTurn()
        {
            base.TickAfterEnemiesTurn();

            if (!_isActive || _owner.IsEnemy)
                return;
            _strenghtEffect.Value = _isOddTurn ? strenghtBuff : strenghtDebuff;
            _intelegenceEffect.Value = _isOddTurn ? intelegenceDebuff : intelegenceBuff;
            _isOddTurn = !_isOddTurn;
        }

        #region IAbleToApplyStrenghtModificator
        public int GetStrenghtModificatorValue() => strenghtBuff;

        public int GetDeltaOfStrenghtValueForEachTurn() => strenghtDebuff;

        public int GetTimeToEndOfStrenghtEffect() => 0;

        public bool GetStrenghtEffectIsConstantly() => true;
        #endregion
        
        #region IAbleToApplyIntelligenceModificator
        public int GetIntelligenceModificatorValue() => intelegenceBuff;

        public int GetDeltaOfIntelligenceValueForEachTurn() => intelegenceDebuff;

        public int GetTimeToEndOfIntelligenceEffect() => 0;

        public bool GetIntelligenceEffectIsConstantly() => true;
        #endregion
    }
}