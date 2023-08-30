using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

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
        private Unit _owner;
        private Dictionary<StatEffectTypes, List<StatEffect>> _statsDictionary;
        private Dictionary<StatEffectTypes, UnityEvent<int>> _statsUpdatedEventsDictionary;
        private UnityEvent<int> _strenghtUpdated, _intelegenceUpdated, _dexterityUpdated, _defenceUpdated, _spikesUpdated;

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
            StatEffect statEffect = new StatEffect(value, timeToTheEndOfEffect);
            _statsDictionary[type].Add(statEffect);
            _statsUpdatedEventsDictionary[type].Invoke(value);
            statEffect.OnEffectValueChanged.AddListener(_ => _statsUpdatedEventsDictionary[type].Invoke(value));
            return statEffect;
        }

        public void AddStatEffect(StatEffect statEffect)
        {
            strenght.Add(statEffect);
        }

        public void RemoveStatEffect(StatEffect statEffect)
        {
            strenght.Remove(statEffect);
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
            strenght.ForEach(effectInstance => effectInstance.Tick());
            intelegence.ForEach(effectInstance => effectInstance.Tick());
            dexterity.ForEach(effectInstance => effectInstance.Tick());
            defence.ForEach(effectInstance => effectInstance.Tick());
            spikes.ForEach(effectInstance => effectInstance.Tick());


            //todo remove elemenes on their timeToEnd >= 0
            for(int i = 0; i < strenght.Count; i++)
            {

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
                { StatEffectTypes.strenght, _strenghtUpdated },
                { StatEffectTypes.intelegence, _intelegenceUpdated },
                { StatEffectTypes.dexterity, _dexterityUpdated },
                { StatEffectTypes.defence, _defenceUpdated },
                { StatEffectTypes.spikes, _spikesUpdated }
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
    public struct StatEffect
    {
        private int value;
        public int timeToTheEndOfEffect;
        public UnityEvent<int> OnEffectValueChanged;

        public StatEffect(int value, int timeToTheEndOfEffect)
        {
            this.value = value;
            this.timeToTheEndOfEffect = timeToTheEndOfEffect;
            OnEffectValueChanged = new UnityEvent<int>();
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

                OnEffectValueChanged.Invoke(delta);
            }
        }
    }
}