using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : BaseInput
{
    [SerializeField] private VisualizeSelectedCell selectedCellVisualizer;
    [SerializeField] private UnitsUIEnabler unitsUIEnabler;
    [SerializeField] private IntContainer preCostContainer;

    [Header("Movement Input")]
    [SerializeField] private HighlightValidateCells movementCellsHighlighter;
    [SerializeField] private FindWayInValidCells findWayInValidCells;
    [SerializeField] private VisualizePath pathVisualizer;

    [Header("Research Input")]
    [SerializeField] private Weapon inspectAbility;
    [SerializeField] private Weapon currentAbility;

    private List<Cell> _path = new();
    private bool _inputIsPossible;
    private float bottomUiPanelHeight = 120f;

    public bool InputIsPossible
    {
        set => _inputIsPossible = value;
        get => _inputIsPossible;
    }

    public override void Act()
    {
        _inputIsPossible = true;
    }

    private void Update()
    {
        DisableAllVisualizationFromPlayerOnMap();
        //DisableDescriptionPanel();
        unitsUIEnabler.AllUnitsUISetActive(false);
        
        if (!UnitsQueue.Instance.IsUnitCurrent(unit) || !_inputIsPossible || CurrentlyActiveObjects.SomethingIsActNow)
            return;

        if (_path.Count != 0)
        {
            _inputIsPossible = false;

            unit.movable.Move(_path[0]);
            _path.RemoveAt(0);
            return;
        }

        selectedCellVisualizer.ApplyEffect();
        ResetPreCostContainers();

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (currentAbility != null)
            {
                currentAbility = null;
            }
            else
            {
                currentAbility = inspectAbility;
            }
        }

        if (currentAbility != null)
        {
            AbilityInput(ref currentAbility);
        }
        else
        {
            MovementInput();
        }
        
        unitsUIEnabler.AllUnitsUISetActive(true);
    }

    public void DisableAllVisualizationFromPlayerOnMap()
    {
        Map.Instance.allCells.ForEach(cell => cell.DisableAllCellVisualization());
        var cellsWithContent = Map.Instance.allCells.WithContentOnly();
        cellsWithContent.Where(cell => cell.Content.health != null).ToList().ForEach(cell => cell.Content.health.ResetPreDamageValue());
        cellsWithContent.Where(cell => cell.Content.pushable != null).ToList().ForEach(cell => cell.Content.pushable.ResetPrePushValue());
    }

    private void ResetPreCostContainers()
    {
        preCostContainer.Content = 0;
    }

    private void MovementInput()
    {
        pathVisualizer.ApplyEffect();
        movementCellsHighlighter.ApplyEffect();

        if (findWayInValidCells.Take() != null)
            preCostContainer.Content = findWayInValidCells.Take().Count - 1;
        else
            preCostContainer.Content = 0;

        if (Input.GetKeyDown(KeyCode.Mouse0) && Input.mousePosition.y > bottomUiPanelHeight)
        {
            _path = findWayInValidCells.Take() == null ? new List<Cell>() : findWayInValidCells.Take();
            preCostContainer.Content = 0;
        }
    }

    private void AbilityInput(ref Weapon ability)
    {
        ability.HighlightCells();
        preCostContainer.Content = ability.CurrentActionCost;

        if (Input.GetKeyDown(KeyCode.Mouse0) && Input.mousePosition.y > bottomUiPanelHeight)
        {
            ability.Use();
            ability = null;
        }
    }

    //private void DisableDescriptionPanel() => printUnitsDescriptionEffect.ApplyEffect(new List<Cell>());

    public void SetCurrentAbility(AOEWeapon ability) => currentAbility = ability;
}
