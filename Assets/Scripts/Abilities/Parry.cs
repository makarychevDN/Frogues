using UnityEngine;

namespace FroguesFramework
{
    public class Parry : MonoBehaviour, IAbility, IAbleToDrawAbilityButton
    {
        [SerializeField] private int temporaryArmorIncreaseValue;
        [SerializeField] private AbilityDataForButton abilityDataForButton;
        private Unit _unit;
        private ActionPoints _actionPoints;
        
        public void VisualizePreUse()
        {
            
        }

        public void Use()
        {
            _unit.Health.IncreaseTemporaryArmor(temporaryArmorIncreaseValue);
        }

        public void Init(Unit unit)
        {
            _unit = unit;
            _actionPoints = unit.ActionPoints;
            unit.AbilitiesManager.AddAbility(this);
        }

        public int GetCost()
        {
            throw new System.NotImplementedException();
        }

        public bool IsPartOfWeapon() => false;

        public AbilityDataForButton GetAbilityDataForButton() => abilityDataForButton;
    }
}