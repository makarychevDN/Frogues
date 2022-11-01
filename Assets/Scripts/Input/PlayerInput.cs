using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class PlayerInput : BaseInput
    {
        private Unit unit;
        private MovementAbility _movementAbility;
        private IAbility _currentAbility;
        private float bottomUiPanelHeight = 120f;

        #region GetSet

        public Unit Unit
        {
            get => unit;
            set => unit = value;
        }

        public MovementAbility MovementAbility
        {
            get => _movementAbility;
            set => _movementAbility = value;
        }

        public IAbility CurrentAbility
        {
            get => _currentAbility;
            set => _currentAbility = value;
        }

        #endregion
        
        public bool InputIsPossible => UnitsQueue.Instance.IsUnitCurrent(unit)
                                       && !CurrentlyActiveObjects.SomethingIsActNow;

        public override void Act(){}

        private void Update()
        {
            DisableAllVisualizationFromPlayerOnMap();

            if (!InputIsPossible)
                return;

            if (_currentAbility == null)
                _currentAbility = _movementAbility;

            AbilityInput(_currentAbility);
        }

        private void AbilityInput(IAbility ability)
        {
            ability.VisualizePreUse();

            if (Input.GetKeyDown(KeyCode.Mouse0) && Input.mousePosition.y > bottomUiPanelHeight)
            {
                ability.Use();
            }
        }

        public void DisableAllVisualizationFromPlayerOnMap()
        {
            Map.Instance.allCells.ForEach(cell => cell.DisableAllCellVisualization());
            var cellsWithContent = Map.Instance.allCells.WithContentOnly();
            cellsWithContent.Where(cell => cell.Content.health != null).ToList()
                .ForEach(cell => cell.Content.health.ResetPreDamageValue());
            cellsWithContent.Where(cell => cell.Content.pushable != null).ToList()
                .ForEach(cell => cell.Content.pushable.ResetPrePushValue());
            cellsWithContent.Where(cell => cell.Content.actionPoints != null).ToList()
                .ForEach(cell => cell.Content.actionPoints.ResetPreCostValue());
        }

        public void SetCurrentAbility(IAbility ability) => _currentAbility = ability;

        public void ClearCurrentAbility() => _currentAbility = null;
    }
}
