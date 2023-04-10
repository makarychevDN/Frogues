using UnityEngine;

namespace FroguesFramework
{
    public class Parry : MonoBehaviour, IAbility, IAbleToDrawAbilityButton
    {
        [SerializeField] private int cost;
        [SerializeField] private int temporaryBlockIncreaseValue;
        [SerializeField] private AbilityDataForButton abilityDataForButton;
        private Unit _unit;
        private Animator _animator;
        private ActionPoints _actionPoints;
        
        public void VisualizePreUse()
        {
            _actionPoints.PreSpendPoints(cost);
        }

        public void Use()
        {
            //CurrentlyActiveObjects.Add(this);
            //_animator.SetTrigger(CharacterAnimatorParameters.Cast);
            //Invoke(nameof(ApplyEffect), animationBeforeImpactTime);
            //Invoke(nameof(RemoveThisFromCurrentlyActiveObjects), fullAnimationTime);
            if (!_actionPoints.IsActionPointsEnough(cost))
                return;

            ApplyEffect();
        }

        public void Init(Unit unit)
        {
            _unit = unit;
            _actionPoints = unit.ActionPoints;
            unit.AbilitiesManager.AddAbility(this);
            _animator = _unit.Animator;
        }

        public int GetCost() => cost;

        public bool IsPartOfWeapon() => true;

        public AbilityDataForButton GetAbilityDataForButton() => abilityDataForButton;

        private void ApplyEffect()
        {
            _unit.Health.IncreaseTemporaryArmor(temporaryBlockIncreaseValue);
            _actionPoints.SpendPoints(cost);
        }

        private void RemoveThisFromCurrentlyActiveObjects() => CurrentlyActiveObjects.Remove(this);
    }
}