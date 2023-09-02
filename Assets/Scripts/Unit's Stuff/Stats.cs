using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

namespace FroguesFramework
{
    public class Stats : MonoBehaviour, IAbleToCalculateHashFunctionOfPrevisualisation, IRoundTickable, IInitializeable
    {
        [SerializeField] private List<StatEffect> strenght;
        [SerializeField] private List<StatEffect> intelegence;
        [SerializeField] private List<StatEffect> dexterity;
        [SerializeField] private List<StatEffect> defence;
        [SerializeField] private List<StatEffect> spikes;
        [SerializeField] private float strengtModificatorStep;
        [SerializeField] private float intelegenceModificatorStep;
        [SerializeField] private float dexterityModificatorStep;
        [SerializeField] private float defenceModificatorStep;
        public UnityEvent<int> OnStrenghtUpdated, OnIntelegenceUpdated, OnDexterityUpdated, OnDefenceUpdated, OnSpikesUpdated;
        private Unit _owner;
        private Dictionary<StatEffectTypes, List<StatEffect>> _statsDictionary = new();
        private Dictionary<StatEffectTypes, UnityEvent<int>> _statsUpdatedEventsDictionary = new();

        public int Strenght => strenght.Sum(effectInstance => effectInstance.Value);
        public int Intelegence => intelegence.Sum(effectInstance => effectInstance.Value);
        public int Dexterity => dexterity.Sum(effectInstance => effectInstance.Value);
        public int Defence => defence.Sum(effectInstance => effectInstance.Value);
        public int Spikes => spikes.Sum(effectInstance => effectInstance.Value);

        public float StrenghtModificator => (1 + Strenght * strengtModificatorStep);
        public float IntelegenceModificator => (1 + Intelegence * intelegenceModificatorStep);
        public float DexterityeModificator => (1 + Dexterity * dexterityModificatorStep);
        public float DefenceModificator => (1 + Defence * defenceModificatorStep);

        public int CalculateHashFunctionOfPrevisualisation() => Strenght ^ Intelegence ^ Dexterity ^ Defence ^ Spikes;

        public StatEffect AddStatEffect(StatEffectTypes type, int value, int timeToTheEndOfEffect)
        {
            StatEffect statEffect = new StatEffect(type, value, timeToTheEndOfEffect);
            _statsDictionary[type].Add(statEffect);
            _statsUpdatedEventsDictionary[type].Invoke(value);
            statEffect.OnEffectValueChanged.AddListener(InvokeEventByKey);
            return statEffect;
        }

        private void InvokeEventByKey(StatEffectTypes key, int value)
        {
            _statsUpdatedEventsDictionary[key].Invoke(value);
        }

        public void AddStatEffect(StatEffect statEffect)
        {
            _statsDictionary[statEffect.type].Add(statEffect);
            _statsUpdatedEventsDictionary[statEffect.type].Invoke(statEffect.Value);
            statEffect.OnEffectValueChanged.AddListener(InvokeEventByKey);
        }

        public void RemoveStatEffect(StatEffect statEffect)
        {
            statEffect.OnEffectValueChanged.RemoveListener(InvokeEventByKey);
            _statsDictionary[statEffect.type].Remove(statEffect);
            _statsUpdatedEventsDictionary[statEffect.type].Invoke(-statEffect.Value);
        }

        #region timerStuff
        public void TickAfterEnemiesTurn()
        {
            if (!_owner.IsEnemy)
                return;

            TickAllEffects();
        }

        public void TickAfterPlayerTurn()
        {
            if (_owner.IsEnemy)
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
                { StatEffectTypes.spikes, spikes }
            };

            _statsUpdatedEventsDictionary = new Dictionary<StatEffectTypes, UnityEvent<int>>
            {
                { StatEffectTypes.strenght, OnStrenghtUpdated },
                { StatEffectTypes.intelegence, OnIntelegenceUpdated },
                { StatEffectTypes.dexterity, OnDexterityUpdated },
                { StatEffectTypes.defence, OnDefenceUpdated },
                { StatEffectTypes.spikes, OnSpikesUpdated }
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
        spikes = 40
    }

    [Serializable]
    public class StatEffect
    {
        public StatEffectTypes type;
        [SerializeField] private int value;
        public int timeToTheEndOfEffect;
        public UnityEvent<StatEffectTypes, int> OnEffectValueChanged;

        public StatEffect(StatEffectTypes type, int value, int timeToTheEndOfEffect)
        {
            this.type = type;
            this.value = value;
            this.timeToTheEndOfEffect = timeToTheEndOfEffect;
            OnEffectValueChanged = new UnityEvent<StatEffectTypes, int>();
        }

        public void Tick(int value = 1)
        {
            timeToTheEndOfEffect -= value;
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