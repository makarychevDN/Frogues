using System;
using UnityEngine;

namespace FroguesFramework
{
    public class SpearAttackAbility : MonoBehaviour, IAbility
    {
        [SerializeField] private int radius;
        [SerializeField] private int cost;
        private Unit _unit;
        private ActionPoints _actionPoints;
        private Grid _grid;

        public void VisualizePreUse()
        {
            var attackArea = PathFinder.Instance.GetCellsAreaForAOE(_unit.currentCell, radius, false, false);
            attackArea.ForEach(cell => cell.EnableValidForAbilityCellHighlight(attackArea));
            
            Vector3Int coordinate = _grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Cell targetCell;
            
            try { targetCell = Map.Instance.layers[MapLayer.DefaultUnit][coordinate.x, coordinate.y]; }
            catch (IndexOutOfRangeException e) { return; }
            
            if (!attackArea.Contains(targetCell))
                return;
            
            targetCell.EnableSelectedCellHighlight(true);
        }

        public void Use()
        {
            
        }
        
        public void Init(Unit unit)
        {
            _unit = unit;
            _actionPoints = unit.actionPoints;
            _grid = unit.Grid;
        }

        [ContextMenu("Init")]
        public void Init() => Init(GetComponentInParent<Unit>());

        [ContextMenu("Set as current ability")]
        public void SetAsCurrentAbility()
        {
            var playersInput = _unit.input as PlayerInput;
            
            if (playersInput == null)
                return;

            playersInput.CurrentAbility = this;
        }
    }
}
