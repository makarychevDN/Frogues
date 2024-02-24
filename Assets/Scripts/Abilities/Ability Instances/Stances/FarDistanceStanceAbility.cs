using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class FarDistanceStanceAbility : BattleStanceAbility, IAbleToApplyStrenghtModificator, IAbleToApplyIntelligenceModificator, IAbleToApplyDexterityModificator, IAbleToApplyDefenceModificator
    {
        [SerializeField] private List<StatEffectAndDeltaWhenOnlyNoEnemieNearby> startEffectsAndDeltas;
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

                foreach (var effect in _effects)
                {
                    _owner.Stats.AddStatEffect(effect);
                }
            }
            else
            {
                EntryPoint.Instance.OnSomeoneMoved.RemoveListener(RecalculateValue);

                foreach (var effect in _effects)
                {
                    _owner.Stats.RemoveStatEffect(effect);
                }
            }
        }

        private void RecalculateValue()
        {
            int nearbyEnemiesQuantity = _owner.CurrentCell.CellNeighbours.GetAllNeighbors().Where(cell => cell.Content != null && cell.Content is not Barrier).Count();
            int noOneEnemyNearbyModificator = nearbyEnemiesQuantity == 0 ? 1 : 0;

            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].Value = startEffectsAndDeltas[i].startValue.Value + noOneEnemyNearbyModificator * startEffectsAndDeltas[i].deltaForNoEnemyNearby;
            }


        }

        #region IAbleToApplyStrenghtModificator
        public int GetStrenghtModificatorValue() => startEffectsAndDeltas.FirstOrDefault(statEffectAndDelta => statEffectAndDelta.startValue.type == StatEffectTypes.strenght).startValue.Value;

        public int GetDeltaOfStrenghtValueForEachTurn()
        {
            int delta = startEffectsAndDeltas.FirstOrDefault(statEffectAndDelta => statEffectAndDelta.startValue.type == StatEffectTypes.strenght).deltaForNoEnemyNearby;
            return delta - Mathf.Abs(GetStrenghtModificatorValue());
        }

        public int GetTimeToEndOfStrenghtEffect() => int.MaxValue;

        public bool GetStrenghtEffectIsConstantly() => true;
        #endregion

        #region IAbleToApplyDexterityModificator
        public int GetDexterityModificatorValue() => startEffectsAndDeltas.FirstOrDefault(statEffectAndDelta => statEffectAndDelta.startValue.type == StatEffectTypes.dexterity).startValue.Value;

        public int GetDeltaOfDexterityValueForEachTurn()
        {
            int delta = startEffectsAndDeltas.FirstOrDefault(statEffectAndDelta => statEffectAndDelta.startValue.type == StatEffectTypes.dexterity).deltaForNoEnemyNearby;
            return delta - Mathf.Abs(GetDexterityModificatorValue());
        }

        public int GetTimeToEndOfDexterityEffect() => int.MaxValue;

        public bool GetDexterityEffectIsConstantly() => true;
        #endregion

        #region #IAbleToApplyIntelligenceModificator
        public int GetIntelligenceModificatorValue() => startEffectsAndDeltas.FirstOrDefault(statEffectAndDelta => statEffectAndDelta.startValue.type == StatEffectTypes.intelegence).startValue.Value;

        public int GetDeltaOfIntelligenceValueForEachTurn()
        {
            int delta = startEffectsAndDeltas.FirstOrDefault(statEffectAndDelta => statEffectAndDelta.startValue.type == StatEffectTypes.intelegence).deltaForNoEnemyNearby;
            return delta - Mathf.Abs(GetIntelligenceModificatorValue());
        }

        public int GetTimeToEndOfIntelligenceEffect() => int.MaxValue;

        public bool GetIntelligenceEffectIsConstantly() => true;
        #endregion


        public int GetDefenceModificatorValue() => startEffectsAndDeltas.FirstOrDefault(statEffectAndDelta => statEffectAndDelta.startValue.type == StatEffectTypes.defence).startValue.Value;

        public int GetdeltaOfDefenceValueForEachTurn()
        {
            int delta = startEffectsAndDeltas.FirstOrDefault(statEffectAndDelta => statEffectAndDelta.startValue.type == StatEffectTypes.defence).deltaForNoEnemyNearby;
            return delta - Mathf.Abs(GetDefenceModificatorValue());
        }

        public int GetTimeToEndOfDefenceEffect() => int.MaxValue;

        public bool GetDefenceEffectIsConstantly() => true;
    }

    [Serializable]
    public class StatEffectAndDeltaWhenOnlyNoEnemieNearby
    {
        public int deltaForNoEnemyNearby;
        public StatEffect startValue;
    }
}