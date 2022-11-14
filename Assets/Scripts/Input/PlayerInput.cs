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

        public bool InputIsPossible => UnitsQueue.Instance.IsUnitCurrent(_unit)
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
            Map.Instance.allCells.ForEach(cell => cell.DisableAllCellVisualization());
            var cellsWithContent = Map.Instance.allCells.WithContentOnly();
            cellsWithContent.Where(cell => cell.Content.health != null).ToList()
                .ForEach(cell => cell.Content.health.ResetPreDamageValue());
            cellsWithContent.Where(cell => cell.Content.actionPoints != null).ToList()
                .ForEach(cell => cell.Content.actionPoints.ResetPreCostValue());
        }

        public void SetCurrentAbility(IAbility ability) => _currentAbility = ability;

        public IAbility GetCurrentAbility() => _currentAbility;

        public void ClearCurrentAbility() => _currentAbility = null;
    }
}
