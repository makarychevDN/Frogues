using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class PlayerInput : BaseInput
{
    [SerializeField, ReadOnly] private InputType _currentInput = InputType.movement;
    [SerializeField] private VisualizeSelectedCell selectedCellVisualizer;

    [Header("Movement Input")]
    [SerializeField] private HighlightValidateCells movementCellsHighlighter;
    [SerializeField] private FindWayInValidCells findWayInValidCells;
    [SerializeField] private VisualizePath pathVisualizer;
    [SerializeField] private IntContainer movementPreCost;

    [Header("Push Input")]
    [SerializeField] private Weapon kick;
    [SerializeField] private IntContainer pushtPreCost;

    [Header("Weapon Input")]
    [SerializeField] private Weapon weapon;
    [SerializeField] private IntContainer weaponPreCost;

    private List<Cell> _path = new List<Cell>();
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

        if (!UnitsQueue.Instance.IsUnitCurrent(unit))
            return;

        if (!_inputIsPossible || CurrentlyActiveObjects.SomethingIsActNow)
            return;

        if (_path.Count != 0)
        {
            _inputIsPossible = false;

            unit.movable.Move(_path[0]);
            _path.RemoveAt(0);
            return;
        }

        selectedCellVisualizer.ApplyEffect();

        movementPreCost.Content = 0;
        weaponPreCost.Content = 0;
        pushtPreCost.Content = 0;

        switch (_currentInput)
        {
            case InputType.movement: MovementInput(); break;
            case InputType.attack: AttackInput(); break;
            case InputType.push: PushInput(); break;
            case InputType.research: ResearchInput(); break;
        }

        ChangeInputTypeInput();
    }

    public void DisableAllVisualizationFromPlayerOnMap()
    {
        Map.Instance.allCells.ForEach(cell => cell.DisableAllCellVisualization());
        var cellsWithContent = Map.Instance.allCells.WithContentOnly();
        cellsWithContent.Where(cell => cell.Content.health != null).ToList().ForEach(cell => cell.Content.health.ResetPreDamageValue());
        cellsWithContent.Where(cell => cell.Content.pushable != null).ToList().ForEach(cell => cell.Content.pushable.ResetPrePushValue());
    }

    private void MovementInput()
    {
        pathVisualizer.ApplyEffect();
        movementCellsHighlighter.ApplyEffect();

        if (findWayInValidCells.Take() != null)
            movementPreCost.Content = findWayInValidCells.Take().Count;
        else
            movementPreCost.Content = 0;

        if (Input.GetKeyDown(KeyCode.Mouse0) && Input.mousePosition.y > bottomUiPanelHeight)
        {
            _path = findWayInValidCells.Take() == null ? new List<Cell>() : findWayInValidCells.Take();
            movementPreCost.Content = 0;
        }
    }

    private void AttackInput()
    {
        weapon.HighlightCells();
        weaponPreCost.Content = weapon.CurrentActionCost;

        if (Input.GetKeyDown(KeyCode.Mouse0) && Input.mousePosition.y > bottomUiPanelHeight)
        {
            weapon.Use();
        }
    }

    private void PushInput()
    {
        kick.HighlightCells();
        pushtPreCost.Content = kick.CurrentActionCost;

        if (Input.GetKeyDown(KeyCode.Mouse0) && Input.mousePosition.y > bottomUiPanelHeight)
        {
            kick.Use();
        }
    }

    private void ResearchInput()
    {
        //coming soon
    }

    private void ChangeInputTypeInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
            _currentInput++;

        _currentInput = (InputType)Mathf.Repeat((int)_currentInput, 4);
    }
}

public enum InputType
{
    movement = 0, attack = 1, push = 2, research = 3
}
