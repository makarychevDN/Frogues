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

        public bool InputIsPossible => Room.Instance.UnitsQueue.IsUnitCurrent(_unit)
                                       && !CurrentlyActiveObjects.SomethingIsActNow;

        public void Act()
        {
            DisableAllVisualizationFromPlayerOnMap();

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
            _unit.AbleToSkipTurn.OnSkipTurn.AddListener(DisableAllVisualizationFromPlayerOnMap);
        }

        private void AbilityInput(IAbility ability)
        {
            ability.VisualizePreUse();

            if (Input.GetKeyDown(KeyCode.Mouse0) && Input.mousePosition.y > bottomUiPanelHeight)
            {
                DisableAllVisualizationFromPlayerOnMap();
                ability.Use();
            }
        }

        private void DisableAllVisualizationFromPlayerOnMap()
        {
            Room.Instance.Map.allCells.ForEach(cell => cell.DisableAllCellVisualization());
            var cellsWithContent = Room.Instance.Map.allCells.WithContentOnly();
            cellsWithContent.Where(cell => cell.Content.Health != null).ToList()
                .ForEach(cell => cell.Content.Health.ResetPreDamageValue());
            cellsWithContent.Where(cell => cell.Content.ActionPoints != null).ToList()
                .ForEach(cell => cell.Content.ActionPoints.ResetPreCostValue());
        }

        public void SetCurrentAbility(IAbility ability) => _currentAbility = ability;

        public IAbility GetCurrentAbility() => _currentAbility;

        public void ClearCurrentAbility() => _currentAbility = null;
    }
}
