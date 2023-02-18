using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class PlayerInput : MonoBehaviour, IAbleToAct, IAbleToHaveCurrentAbility
    {
        private Unit _unit;
        private MovementAbility _movementAbility;
        private IAbility _currentAbility;
        private float bottomUiPanelHeight = 120f;

        public bool InputIsPossible => EntryPoint.Instance.UnitsQueue.IsUnitCurrent(_unit)
                                       && !CurrentlyActiveObjects.SomethingIsActNow;

        public void Act()
        {
            if (_currentAbility == null)
                _currentAbility = _movementAbility;

            AbilityInput(_currentAbility);
            
            if(Input.GetKeyDown(KeyCode.Mouse1))
                ClearCurrentAbility();
        }

        public void Init()
        {
            _unit = GetComponentInParent<Unit>();
            _movementAbility = _unit.MovementAbility;
            _currentAbility = _movementAbility;
        }

        private void AbilityInput(IAbility ability)
        {
            ability.VisualizePreUse();

            if (Input.GetKeyDown(KeyCode.Mouse0) && Input.mousePosition.y > bottomUiPanelHeight)
            {
                ability.Use();
            }
        }

        public void SetCurrentAbility(IAbility ability) => _currentAbility = ability;

        public IAbility GetCurrentAbility() => _currentAbility;

        public void ClearCurrentAbility() => _currentAbility = null;
    }
}
