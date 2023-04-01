using FroguesFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class InspectAbility : MonoBehaviour, IAbility, IAbleToDrawAbilityButton, IAbleToDisablePreVisualization
    {
        [SerializeField] private AbilityDataForButton abilityDataForButton;
        private Cell _targetCell;
        private List<Cell> _attackArea;

        public void VisualizePreUse()
        {
            _attackArea = CellsTaker.TakeAllCells();
            _attackArea.ForEach(cell => cell.EnableValidForAbilityCellHighlight(_attackArea));
            _targetCell = CellsTaker.TakeCellByMouseRaycast();

            if (!_attackArea.Contains(_targetCell))
                return;

            _targetCell.EnableSelectedCellHighlight(true);

            if (!_targetCell.IsEmpty)
                EntryPoint.Instance.UnitDescriptionPanel.Activate(_targetCell.Content.UnitDescription.Description);
        }

        public void Use()
        {
            return;
        }

        private void RemoveFromCurrentlyActiveList() => CurrentlyActiveObjects.Remove(this);

        public void Init(Unit unit)
        {
            unit.AbilitiesManager.AddAbility(this);
        }

        public int GetCost() => 0;

        public bool IsPartOfWeapon() => false;

        public AbilityDataForButton GetAbilityDataForButton() => abilityDataForButton;

        public void DisablePreVisualization()
        {
            print("unyay");
        }
    }
}