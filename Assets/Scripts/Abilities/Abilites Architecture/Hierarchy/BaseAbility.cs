using UnityEngine;

namespace FroguesFramework
{
    public abstract class BaseAbility : MonoBehaviour, IInitializeable, IAbleToDrawAbilityButton
    {
        [SerializeField] private AbilityDataForButton abilityDataForButton;
        [SerializeField] private bool isPartOfWeapon;
        protected Unit _owner;

        public bool HasOwner => _owner != null;
        public bool IsPartOfWeapon => isPartOfWeapon;
        public AbilityDataForButton GetAbilityDataForButton() => abilityDataForButton;

        public virtual void Init(Unit unit) 
        { 
            _owner = unit;
            _owner.AbilitiesManager.AddAbility(this);
        }

        public virtual void UnInit() => _owner = null;

        public virtual bool IsIgnoringDrawingFunctionality() => false;

        private void Reset()
        {
            abilityDataForButton = GetComponent<AbilityDataForButton>();
        }
    }
}