using UnityEngine;

namespace FroguesFramework
{
    public abstract class BaseAbility : MonoBehaviour, IInitializeable, IAbleToDrawAbilityButton
    {
        [SerializeField] private AbilityDataForButton abilityDataForButton;
        protected Unit _owner;

        public AbilityDataForButton GetAbilityDataForButton() => abilityDataForButton;
        public virtual void Init(Unit unit) => _owner = unit;
        public virtual void UnInit() => _owner = null;
    }
}