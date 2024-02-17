using UnityEngine;

namespace FroguesFramework
{
    public abstract class BaseAbility : MonoBehaviour, IInitializeableAbility, IAbleToDrawAbilityButton
    {
        [SerializeField] private AbilityDataForButton abilityDataForButton;
        [SerializeField] private bool isPartOfWeapon;
        protected Unit _owner;

        public bool HasOwner => _owner != null;
        public bool IsPartOfWeapon => isPartOfWeapon;
        public AbilityDataForButton GetAbilityDataForButton() => abilityDataForButton;

        public virtual void Init(Unit unit, bool unitBecomesAnOwner = true) 
        { 
            _owner = unit;

            if(unitBecomesAnOwner)
                _owner.AbilitiesManager.AddAbility(this);
        }

        public virtual void Init(Unit unit) => Init(unit, true);

        public virtual void UnInit() => _owner = null;

        public virtual bool IsIgnoringDrawingFunctionality() => false;

        private void Reset()
        {
            abilityDataForButton = GetComponent<AbilityDataForButton>();
        }
    }
}