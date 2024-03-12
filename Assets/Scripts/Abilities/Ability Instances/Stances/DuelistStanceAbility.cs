using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class DuelistStanceAbility : BattleStanceAbility, IAbleToApplyStrenghtModificator, IAbleToApplyIntelligenceModificator, IAbleToApplyDexterityModificator
    {
        [SerializeField] private List<StatEffectAndDeltaWhenOnlyOneEnemieNearby> startEffectsAndDeltas;
        private List<StatEffect> _effects;

        public override void Init(Unit unit)
        {
            base.Init(unit);

            _effects = new List<StatEffect>();
            for (int i = 0; i < startEffectsAndDeltas.Count; i++)
            {
                var startValue = startEffectsAndDeltas[i].startValue;
                _effects.Add(new StatEffect(startValue.type, startValue.Value, startValue.timeToTheEndOfEffect, startValue.deltaValueForEachTurn, startValue.effectIsConstantly));
            }
        }

        public override void ApplyEffect(bool isActive)
        {
            base.ApplyEffect(isActive);

            if (isActive)
            {
                RecalculateValue();
                EntryPoint.Instance.OnSomeoneMoved.AddListener(RecalculateValue);
                EntryPoint.Instance.OnSomeoneDied.AddListener(RecalculateValue);

                foreach (var effect in _effects)
                {
                    _owner.Stats.AddStatEffect(effect);
                }
            }
            else
            {
                EntryPoint.Instance.OnSomeoneMoved.RemoveListener(RecalculateValue);
                EntryPoint.Instance.OnSomeoneDied.RemoveListener(RecalculateValue);

                foreach (var effect in _effects)
                {
                    _owner.Stats.RemoveStatEffect(effect);
                }
            }
        }

        private void RecalculateValue()
        {
            int nearbyEnemiesQuantity = _owner.CurrentCell.CellNeighbours.GetAllNeighbors().Where(cell => cell.Content is not Barrier && cell.Content != null && cell.Content.IsEnemy).Count();
            int onlyOneEnemieNearbyModificator = nearbyEnemiesQuantity == 1 ? 1 : 0;

            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].Value = startEffectsAndDeltas[i].startValue.Value + onlyOneEnemieNearbyModificator * startEffectsAndDeltas[i].deltaForOneEnemyNearby;
            }
        }

        #region IAbleToApplyStrenghtModificator
        public int GetStrenghtModificatorValue() => startEffectsAndDeltas.FirstOrDefault(statEffectAndDelta => statEffectAndDelta.startValue.type == StatEffectTypes.strength).startValue.Value;

        public int GetDeltaOfStrenghtValueForEachTurn()
        {
            int delta = startEffectsAndDeltas.FirstOrDefault(statEffectAndDelta => statEffectAndDelta.startValue.type == StatEffectTypes.strength).deltaForOneEnemyNearby;
            return delta - Mathf.Abs(GetStrenghtModificatorValue());
        }

        public int GetTimeToEndOfStrenghtEffect() => int.MaxValue;

        public bool GetStrenghtEffectIsConstantly() => true;
        #endregion

        #region IAbleToApplyDexterityModificator
        public int GetDexterityModificatorValue() => startEffectsAndDeltas.FirstOrDefault(statEffectAndDelta => statEffectAndDelta.startValue.type == StatEffectTypes.dexterity).startValue.Value;

        public int GetDeltaOfDexterityValueForEachTurn()
        {
            int delta = startEffectsAndDeltas.FirstOrDefault(statEffectAndDelta => statEffectAndDelta.startValue.type == StatEffectTypes.dexterity).deltaForOneEnemyNearby;
            return delta - Mathf.Abs(GetDexterityModificatorValue());
        }

        public int GetTimeToEndOfDexterityEffect() => int.MaxValue;

        public bool GetDexterityEffectIsConstantly() => true;
        #endregion

        #region #IAbleToApplyIntelligenceModificator
        public int GetIntelligenceModificatorValue() => startEffectsAndDeltas.FirstOrDefault(statEffectAndDelta => statEffectAndDelta.startValue.type == StatEffectTypes.intelligence).startValue.Value;

        public int GetDeltaOfIntelligenceValueForEachTurn()
        {
            int delta = startEffectsAndDeltas.FirstOrDefault(statEffectAndDelta => statEffectAndDelta.startValue.type == StatEffectTypes.intelligence).deltaForOneEnemyNearby;
            return delta - Mathf.Abs(GetIntelligenceModificatorValue());
        }

        public int GetTimeToEndOfIntelligenceEffect() => int.MaxValue;

        public bool GetIntelligenceEffectIsConstantly() => true;
        #endregion
    }

    [Serializable]
    public class StatEffectAndDeltaWhenOnlyOneEnemieNearby
    {
        public int deltaForOneEnemyNearby;
        public StatEffect startValue;
    }
}