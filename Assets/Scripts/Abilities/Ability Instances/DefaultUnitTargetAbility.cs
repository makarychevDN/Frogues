using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class DefaultUnitTargetAbility : UnitTargetAbility, IAbleToBeNativeAttack, IAbleToReturnIsPrevisualized, IAbleToReturnRange, IAbleToDealDamage, IAbleToApplyStatEffects, 
        IAbleToApplyDefenceModificator, IAbleToApplyStrenghtModificator, IAbleToApplyIntelligenceModificator, IAbleToApplyDexterityModificator, IAbleToApplySpikesModificator, IAbleToApplyImmobilizedModificator
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

        public virtual int CalculateDamage() => Extensions.CalculateOutgoingDamageWithGameRules(DamageValue, damageType, _owner.Stats);

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
        public int GetDefenceModificatorValue() => Extensions.GetModificatorValue(addtionalDebufs, StatEffectTypes.defence);

        public int GetdeltaOfDefenceValueForEachTurn() => Extensions.GetDeltaValueOfModificatorForEachTurn(addtionalDebufs, StatEffectTypes.defence);

        public int GetTimeToEndOfDefenceEffect() => Extensions.GetTimeToEndOfEffect(addtionalDebufs, StatEffectTypes.defence);

        public bool GetDefenceEffectIsConstantly() => Extensions.GetEffectIsConstantly(addtionalDebufs, StatEffectTypes.defence);
        #endregion

        #region IAbleToApplyStrenghtModificator
        public int GetStrenghtModificatorValue() => Extensions.GetModificatorValue(addtionalDebufs, StatEffectTypes.strenght);

        public int GetDeltaOfStrenghtValueForEachTurn() => Extensions.GetDeltaValueOfModificatorForEachTurn(addtionalDebufs, StatEffectTypes.strenght);

        public int GetTimeToEndOfStrenghtEffect() => Extensions.GetTimeToEndOfEffect(addtionalDebufs, StatEffectTypes.strenght);

        public bool GetStrenghtEffectIsConstantly() => Extensions.GetEffectIsConstantly(addtionalDebufs, StatEffectTypes.strenght);
        #endregion

        #region IAbleToApplyIntelligenceModificator
        public int GetIntelligenceModificatorValue() => Extensions.GetModificatorValue(addtionalDebufs, StatEffectTypes.intelegence);

        public int GetDeltaOfIntelligenceValueForEachTurn() => Extensions.GetDeltaValueOfModificatorForEachTurn(addtionalDebufs, StatEffectTypes.intelegence);

        public int GetTimeToEndOfIntelligenceEffect() => Extensions.GetTimeToEndOfEffect(addtionalDebufs, StatEffectTypes.intelegence);

        public bool GetIntelligenceEffectIsConstantly() => Extensions.GetEffectIsConstantly(addtionalDebufs, StatEffectTypes.intelegence);
        #endregion

        #region IAbleToApplyDexterityModificator
        public int GetDexterityModificatorValue() => Extensions.GetModificatorValue(addtionalDebufs, StatEffectTypes.dexterity);

        public int GetDeltaOfDexterityValueForEachTurn() => Extensions.GetDeltaValueOfModificatorForEachTurn(addtionalDebufs, StatEffectTypes.dexterity);

        public int GetTimeToEndOfDexterityEffect() => Extensions.GetTimeToEndOfEffect(addtionalDebufs, StatEffectTypes.dexterity);

        public bool GetDexterityEffectIsConstantly() => Extensions.GetEffectIsConstantly(addtionalDebufs, StatEffectTypes.dexterity);
        #endregion

        #region IAbleToApplySpikesModificator
        public int GetSpikesModificatorValue() => Extensions.GetModificatorValue(addtionalDebufs, StatEffectTypes.spikes);

        public int GetdeltaOfSpikesValueForEachTurn() => Extensions.GetDeltaValueOfModificatorForEachTurn(addtionalDebufs, StatEffectTypes.spikes);

        public int GetTimeToEndOfSpikesEffect() => Extensions.GetTimeToEndOfEffect(addtionalDebufs, StatEffectTypes.spikes);

        public bool GetSpikesEffectIsConstantly() => Extensions.GetEffectIsConstantly(addtionalDebufs, StatEffectTypes.spikes);
        #endregion

        #region IAbleToApplyImmobilizedModificator
        public int GetTimeToEndOfImmpobilizedEffect() => Extensions.GetTimeToEndOfEffect(addtionalDebufs, StatEffectTypes.immobilized);
    }
        #endregion
}
