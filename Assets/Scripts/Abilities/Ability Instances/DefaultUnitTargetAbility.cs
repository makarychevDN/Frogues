using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class DefaultUnitTargetAbility : UnitTargetAbility, IAbleToBeNativeAttack, IAbleToReturnIsPrevisualized, IAbleToReturnRange, IAbleToDealDamage, IAbleToApplyStatEffects, 
        IAbleToApplyDefenceModificator, IAbleToApplyStrenghtModificator, IAbleToApplyIntelligenceModificator, IAbleToApplyDexterityModificator, IAbleToApplySpikesModificator
    {
        [SerializeField] protected DamageType damageType;
        [SerializeField] protected int damage;
        [SerializeField] protected int radius;
        [SerializeField] protected bool isNativeAttack;
        [SerializeField] protected bool ignoreArmor;
        [SerializeField] protected bool shouldUseWeapondamageInstead;
        [SerializeField] protected bool shouldUseWeaponActionPointsCostInstead;
        [SerializeField] protected bool shouldSetMyDamageAndCostAsWeaponCharacteristics;
        [SerializeField] protected List<StatEffect> addtionalDebufs;

        [Header("Previsualization Setup")]
        [SerializeField] protected LineRenderer lineFromOwnerToTarget;

        protected bool _isPrevisualizedNow;
        private Unit _hashedTarget;
        public UnityEvent OnEffectApplied;

        private int DamageValue => shouldUseWeapondamageInstead ? _owner.AbilitiesManager.WeaponDamage : damage;

        public virtual int CalculateDamage() => Extensions.CalculateDamageWithGameRules(DamageValue, damageType, _owner.Stats);

        protected override int CalculateActionPointsCost => shouldUseWeaponActionPointsCostInstead ? _owner.AbilitiesManager.WeaponActionPointsCost : actionPointsCost;

        public override List<Cell> CalculateUsingArea() => _usingArea = CellsTaker.TakeCellsAreaByRange(_owner.CurrentCell, radius);

        public override bool PossibleToUseOnUnit(Unit target)
        {
            if(target == null) 
                return false;

            return IsResoursePointsEnough() && _usingArea.Contains(target.CurrentCell);
        }

        public override void PrepareToUsing(Unit target)
        {
            _hashedTarget = target;
            CalculateUsingArea();
        }

        public override void UseOnUnit(Unit target)
        {
            if (!PossibleToUseOnUnit(target))
                return;

            SpendResourcePoints();
            SetCooldownAsAfterUse();

            if (needToRotateOwnersSprite) _owner.SpriteRotator.TurnAroundByTarget(target);
            _owner.Animator.SetTrigger(abilityAnimatorTrigger.ToString());

            CurrentlyActiveObjects.Add(this);
            StartCoroutine(ApplyEffect(timeBeforeImpact, target));
            Invoke(nameof(RemoveCurremtlyActive), fullAnimationTime);
            Invoke(nameof(PlayImpactSound), delayBeforeImpactSound);
        }

        protected virtual IEnumerator ApplyEffect(float time, Unit target)
        {
            yield return new WaitForSeconds(time);

            target.Health.TakeDamage(CalculateDamage(), ignoreArmor, _owner);
            foreach (var effect in addtionalDebufs)
            {
                target.Stats.AddStatEffect(new StatEffect(effect.type, effect.Value, effect.timeToTheEndOfEffect, effect.deltaValueForEachTurn, effect.effectIsConstantly));
            }

            OnEffectApplied.Invoke();
        }

        protected void RemoveCurremtlyActive() => CurrentlyActiveObjects.Remove(this);

        protected void PlayImpactSound()
        {
            if(impactSoundSource != null)
                impactSoundSource.Play();
        }

        public override void VisualizePreUseOnUnit(Unit target)
        {
            _isPrevisualizedNow = true;
            _usingArea.ForEach(cell => cell.EnableValidForAbilityCellHighlight(_usingArea));

            if(target != null)
                target.MaterialInstanceContainer.EnableOutline(true);

            if (!PossibleToUseOnUnit(target))
                return;

            target.Health.PreTakeDamage(CalculateDamage(), ignoreArmor);
            _owner.ActionPoints.PreSpendPoints(actionPointsCost);
            _owner.BloodPoints.PreSpendPoints(bloodPointsCost);
            lineFromOwnerToTarget.gameObject.SetActive(true);
            lineFromOwnerToTarget.SetPosition(0, _owner.SpriteParent.position);
            lineFromOwnerToTarget.SetPosition(1, target.SpriteParent.position);
        }

        public override void DisablePreVisualization()
        {
            lineFromOwnerToTarget.gameObject.SetActive(false);
            _isPrevisualizedNow = false;
        }

        public override void Init(Unit unit)
        {
            base.Init(unit);

            if (shouldSetMyDamageAndCostAsWeaponCharacteristics)
            {
                _owner.AbilitiesManager.SetWeaponActionPointsCost(actionPointsCost);
                _owner.AbilitiesManager.SetWeaponDamage(damage);
                _owner.AbilitiesManager.OnWeaponsDamageUpdated.Invoke();
            }

            if (weaponIndex == WeaponIndexes.NoNeedToChangeWeapon)
                return;

            _owner.Animator.SetInteger(CharacterAnimatorParameters.WeaponIndex, (int)weaponIndex);
            _owner.Animator.SetTrigger(CharacterAnimatorParameters.ChangeWeapon);
        }

        public bool IsNativeAttack()
        {
            return isNativeAttack;
        }

        public void SetAsCurrentNativeAttack()
        {
            var ableToHaveNativeAttack = _owner.ActionsInput as IAbleToHaveNativeAttack;
            if (ableToHaveNativeAttack != null)
                ableToHaveNativeAttack.SetCurrentNativeAttack(this);
        }

        public bool IsPrevisualizedNow()
        {
            return _isPrevisualizedNow;
        }

        public override int CalculateHashFunctionOfPrevisualisation()
        {
            int value = _usingArea.Count;

            if (_hashedTarget != null)
                value ^= _hashedTarget.Coordinates.x ^ _hashedTarget.Coordinates.y ^ _hashedTarget.gameObject.name.GetHashCode();

            return value ^ GetHashCode();
        }

        public override bool CheckItUsableOnDefaultUnit() => true;

        public override bool CheckItUsableOnBloodSurfaceUnit() => false;

        public int ReturnRange() => radius;

        public int GetDefaultDamage() => damage;

        public DamageType GetDamageType() => damageType;

        public List<StatEffect> GetStatEffects() => addtionalDebufs;

        #region IAbleToApplyDefenceModificator
        public int GetDefenceModificatorValue()
        {
            StatEffect effect = addtionalDebufs.FirstOrDefault(statEffect => statEffect.type is StatEffectTypes.defence);

            if (effect == null)
                return 0;

            return effect.Value;
        }

        public int GetdeltaOfDefenceValueForEachTurn()
        {
            StatEffect effect = addtionalDebufs.FirstOrDefault(statEffect => statEffect.type is StatEffectTypes.defence);

            if (effect == null)
                return 0;

            return effect.deltaValueForEachTurn;
        }

        public int GetTimeToEndOfDefenceEffect()
        {
            StatEffect effect = addtionalDebufs.FirstOrDefault(statEffect => statEffect.type is StatEffectTypes.defence);

            if (effect == null)
                return 0;

            return effect.timeToTheEndOfEffect;
        }

        public bool GetDefenceEffectIsConstantly()
        {
            StatEffect effect = addtionalDebufs.FirstOrDefault(statEffect => statEffect.type is StatEffectTypes.defence);

            if (effect == null)
                return false;

            return effect.effectIsConstantly;
        }
        #endregion

        #region IAbleToApplyStrenghtModificator
        public int GetStrenghtModificatorValue()
        {
            StatEffect effect = addtionalDebufs.FirstOrDefault(statEffect => statEffect.type is StatEffectTypes.strenght);

            if (effect == null)
                return 0;

            return effect.Value;
        }

        public int GetDeltaOfStrenghtValueForEachTurn()
        {
            StatEffect effect = addtionalDebufs.FirstOrDefault(statEffect => statEffect.type is StatEffectTypes.strenght);

            if (effect == null)
                return 0;

            return effect.deltaValueForEachTurn;
        }

        public int GetTimeToEndOfStrenghtEffect()
        {
            StatEffect effect = addtionalDebufs.FirstOrDefault(statEffect => statEffect.type is StatEffectTypes.strenght);

            if (effect == null)
                return 0;

            return effect.timeToTheEndOfEffect;
        }

        public bool GetStrenghtEffectIsConstantly()
        {
            StatEffect effect = addtionalDebufs.FirstOrDefault(statEffect => statEffect.type is StatEffectTypes.strenght);

            if (effect == null)
                return false;

            return effect.effectIsConstantly;
        }
        #endregion

        #region IAbleToApplyIntelligenceModificator
        public int GetIntelligenceModificatorValue()
        {
            StatEffect effect = addtionalDebufs.FirstOrDefault(statEffect => statEffect.type is StatEffectTypes.intelegence);

            if (effect == null)
                return 0;

            return effect.Value;
        }

        public int GetDeltaOfIntelligenceValueForEachTurn()
        {
            StatEffect effect = addtionalDebufs.FirstOrDefault(statEffect => statEffect.type is StatEffectTypes.intelegence);

            if (effect == null)
                return 0;

            return effect.deltaValueForEachTurn;
        }

        public int GetTimeToEndOfIntelligenceEffect()
        {
            StatEffect effect = addtionalDebufs.FirstOrDefault(statEffect => statEffect.type is StatEffectTypes.intelegence);

            if (effect == null)
                return 0;

            return effect.timeToTheEndOfEffect;
        }

        public bool GetIntelligenceEffectIsConstantly()
        {
            StatEffect effect = addtionalDebufs.FirstOrDefault(statEffect => statEffect.type is StatEffectTypes.intelegence);

            if (effect == null)
                return false;

            return effect.effectIsConstantly;
        }
        #endregion

        #region IAbleToApplyDexterityModificator
        public int GetDexterityModificatorValue()
        {
            StatEffect effect = addtionalDebufs.FirstOrDefault(statEffect => statEffect.type is StatEffectTypes.dexterity);

            if (effect == null)
                return 0;

            return effect.Value;
        }

        public int GetDeltaOfDexterityValueForEachTurn()
        {
            StatEffect effect = addtionalDebufs.FirstOrDefault(statEffect => statEffect.type is StatEffectTypes.dexterity);

            if (effect == null)
                return 0;

            return effect.deltaValueForEachTurn;
        }

        public int GetTimeToEndOfDexterityEffect()
        {
            StatEffect effect = addtionalDebufs.FirstOrDefault(statEffect => statEffect.type is StatEffectTypes.dexterity);

            if (effect == null)
                return 0;

            return effect.timeToTheEndOfEffect;
        }

        public bool GetDexterityEffectIsConstantly()
        {
            StatEffect effect = addtionalDebufs.FirstOrDefault(statEffect => statEffect.type is StatEffectTypes.dexterity);

            if (effect == null)
                return false;

            return effect.effectIsConstantly;
        }
        #endregion

        #region IAbleToApplySpikesModificator
        public int GetSpikesModificatorValue()
        {
            StatEffect effect = addtionalDebufs.FirstOrDefault(statEffect => statEffect.type is StatEffectTypes.spikes);

            if (effect == null)
                return 0;

            return effect.Value;
        }

        public int GetdeltaOfSpikesValueForEachTurn()
        {
            StatEffect effect = addtionalDebufs.FirstOrDefault(statEffect => statEffect.type is StatEffectTypes.spikes);

            if (effect == null)
                return 0;

            return effect.deltaValueForEachTurn;
        }

        public int GetTimeToEndOfSpikesEffect()
        {
            StatEffect effect = addtionalDebufs.FirstOrDefault(statEffect => statEffect.type is StatEffectTypes.spikes);

            if (effect == null)
                return 0;

            return effect.timeToTheEndOfEffect;
        }

        public bool GetSpikesEffectIsConstantly()
        {
            StatEffect effect = addtionalDebufs.FirstOrDefault(statEffect => statEffect.type is StatEffectTypes.spikes);

            if (effect == null)
                return false;

            return effect.effectIsConstantly;
        }
        #endregion
    }
}