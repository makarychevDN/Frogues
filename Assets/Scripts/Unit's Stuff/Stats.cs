using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class Stats : MonoBehaviour, IAbleToCalculateHashFunctionOfPrevisualisation, IRoundTickable, IInitializeable
    {
        [SerializeField] private List<StatEffect> strenght;
        [SerializeField] private List<StatEffect> intelegence;
        [SerializeField] private List<StatEffect> dexterity;
        [SerializeField] private List<StatEffect> defence;
        [SerializeField] private List<StatEffect> spikes;
        [SerializeField] private List<StatEffect> immobilized;
        [SerializeField] private float strengtModificatorStep;
        [SerializeField] private float intelegenceModificatorStep;
        [SerializeField] private float dexterityModificatorStep;
        [SerializeField] private float defenceModificatorStep;
        public UnityEvent<StatEffectTypes, int> OnStrenghtUpdated, OnIntelegenceUpdated, OnDexterityUpdated, OnDefenceUpdated, OnSpikesUpdated, OnImmobilizedUpdated;
        private Unit _owner;
        private Dictionary<StatEffectTypes, List<StatEffect>> _statsDictionary = new();
        private Dictionary<StatEffectTypes, UnityEvent<StatEffectTypes, int>> _statsUpdatedEventsDictionary = new();

        public int Strenght => strenght.GetStatValue();
        public int Intelegence => intelegence.GetStatValue();
        public int Dexterity => dexterity.GetStatValue();
        public int Defence => defence.GetStatValue();
        public int Spikes => spikes.GetStatValue();
        public int Immobilized => immobilized.GetTimeToTheEndOfEffect();

        public float StrenghtModificator => (1 + strenght.GetStatValue() * strengtModificatorStep);
        public float IntelegenceModificator => (1 + intelegence.GetStatValue() * intelegenceModificatorStep);
        public float StrenghtAndIntelligenceSumModificator => (1 + strenght.GetStatValue() * strengtModificatorStep + intelegence.GetStatValue() * intelegenceModificatorStep);
        public float DexterityeModificator => (1 + dexterity.GetStatValue() * dexterityModificatorStep);
        public float DefenceModificator => (1 - defence.GetStatValue() * defenceModificatorStep);

        public float StrenghtModificatorPersentages => strengtModificatorStep * 100;
        public float IntelegenceModificatorPersentages => intelegenceModificatorStep * 100;
        public float DexterityeModificatorPersentages => dexterityModificatorStep * 100;
        public float DefenceModificatorPersentages => defenceModificatorStep * 100;

        public int CalculateHashFunctionOfPrevisualisation() => strenght.GetStatValue() * 1 + intelegence.GetStatValue() * 10 + dexterity.GetStatValue() * 100 + defence.GetStatValue() * 1000 + spikes.GetStatValue() * 10000 + immobilized.GetTimeToTheEndOfEffect() * 100000;

        public StatEffect AddStatEffect(StatEffectTypes type, int value, int timeToTheEndOfEffect, int deltaValueForEachTurn = 0, bool effectIsConstantly = false)
        {
            StatEffect statEffect = new StatEffect(type, value, timeToTheEndOfEffect, deltaValueForEachTurn, effectIsConstantly);
            _statsDictionary[type].Add(statEffect);
            _statsUpdatedEventsDictionary[type].Invoke(type, value);
            statEffect.OnEffectValueChanged.AddListener(InvokeEventByKey);
            return statEffect;
        }

        private void InvokeEventByKey(StatEffectTypes key, int value)
        {
            _statsUpdatedEventsDictionary[key].Invoke(key, value);
        }

        public void AddStatEffect(StatEffect statEffect)
        {
            _statsDictionary[statEffect.type].Add(statEffect);
            _statsUpdatedEventsDictionary[statEffect.type].Invoke(statEffect.type, statEffect.Value);
            statEffect.OnEffectValueChanged.AddListener(InvokeEventByKey);
        }

        public void RemoveStatEffect(StatEffect statEffect)
        {
            statEffect.OnEffectValueChanged.RemoveListener(InvokeEventByKey);
            _statsDictionary[statEffect.type].Remove(statEffect);
            _statsUpdatedEventsDictionary[statEffect.type].Invoke(statEffect.type, - statEffect.Value);
        }

        public void RemoveAllNonConstantlyEffects()
        {
            foreach(var statEffectsList in _statsDictionary.Values)
            {
                statEffectsList.RemoveAll(statEffect => !statEffect.effectIsConstantly);
            }
        }

        #region timerStuff
        public void TickAfterEnemiesTurn()
        {
            if (_owner.IsEnemy)
                return;

            TickAllEffects();
        }

        public void TickAfterPlayerTurn()
        {
            if (!_owner.IsEnemy)
                return;

            TickAllEffects();
        }

        private void TickAllEffects()
        {
            foreach(var key in _statsDictionary.Keys)
            {
                for(int i = 0; i < _statsDictionary[key].Count; i++)
                {
                    _statsDictionary[key][i].Tick();

                    if(_statsDictionary[key][i].timeToTheEndOfEffect <= 0)
                    {
                        RemoveStatEffect(_statsDictionary[key][i]);
                        i--;
                    }
                }
            }
        }

        #endregion

        #region InitStuff
        public void Init(Unit unit)
        {
            _owner = unit;
            _statsDictionary = new Dictionary<StatEffectTypes, List<StatEffect>>
            {
                { StatEffectTypes.strenght, strenght },
                { StatEffectTypes.intelegence, intelegence },
                { StatEffectTypes.dexterity, dexterity },
                { StatEffectTypes.defence, defence },
                { StatEffectTypes.spikes, spikes },
                { StatEffectTypes.immobilized, immobilized }
            };

            _statsUpdatedEventsDictionary = new Dictionary<StatEffectTypes, UnityEvent<StatEffectTypes,int>>
            {
                { StatEffectTypes.strenght, OnStrenghtUpdated },
                { StatEffectTypes.intelegence, OnIntelegenceUpdated },
                { StatEffectTypes.dexterity, OnDexterityUpdated },
                { StatEffectTypes.defence, OnDefenceUpdated },
                { StatEffectTypes.spikes, OnSpikesUpdated },
                { StatEffectTypes.immobilized, OnImmobilizedUpdated }
            };
        }

    public void UnInit(){}
    #endregion

    }

    public enum StatEffectTypes
    {
        strenght = 0,
        intelegence = 10,
        dexterity = 20,
        defence = 30,
        spikes = 40,
        immobilized = 50
    }

    [Serializable]
    public class StatEffect
    {
        public StatEffectTypes type;
        [SerializeField] private int value;
        public int deltaValueForEachTurn;
        public int timeToTheEndOfEffect;
        public bool effectIsConstantly;
        public UnityEvent<StatEffectTypes, int> OnEffectValueChanged;

        public StatEffect(StatEffectTypes type, int value, int timeToTheEndOfEffect, int deltaForEachTurn = 0, bool effectIsConstantly = false)
        {
            this.type = type;
            this.value = value;
            this.timeToTheEndOfEffect = timeToTheEndOfEffect;
            this.effectIsConstantly = effectIsConstantly;
            this.deltaValueForEachTurn = deltaForEachTurn;
            OnEffectValueChanged = new UnityEvent<StatEffectTypes, int>();
        }

        public StatEffect(StatEffect statEffect) : this(statEffect.type, statEffect.value, statEffect.timeToTheEndOfEffect, statEffect.deltaValueForEachTurn, statEffect.effectIsConstantly) { }

        public void Tick(int ticksValue = 1)
        {
            if(!effectIsConstantly)
                timeToTheEndOfEffect -= ticksValue;

            Value += deltaValueForEachTurn;
        }

        public int Value
        {
            get { return value; }

            set
            {
                int delta = value - this.value;
                this.value = value;

                if(delta != 0)
                    OnEffectValueChanged.Invoke(type ,delta);
            }
        }
    }
}