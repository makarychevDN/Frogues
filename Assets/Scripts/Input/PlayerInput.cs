using System.Collections;
using System.Collections.Generic;
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

    [Header("Push Input")] 
    [SerializeField] private HighlightValidateCells pushCellsHighlighter;
    [SerializeField] private BaseCellsTaker validForPushCellsTaker;
    [SerializeField] private HighlightSelectedCells selectedPushCellHighlighter;
    [SerializeField] private CellByMousePosition cellByMousePosition;

    private List<Cell> _path = new List<Cell>();
    private bool _inputIsPossible;

    public override void Act()
    {
        _inputIsPossible = true;
    }

    private void Update()
    {
        if (!_inputIsPossible)
            return;

        movementCellsHighlighter.TurnOffHighlight();
        pathVisualizer.TurnOffVisualization();
        selectedCellVisualizer.TurnOffVizualisation();

        selectedCellVisualizer.ApplyEffect();


        switch (_currentInput)
        {
            case InputType.movement: MovementInput(); break;
            case InputType.attack: AttackInput(); break;
            case InputType.push: PushInput(); break;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
            _currentInput++;

        _currentInput = (InputType)Mathf.Repeat((int)_currentInput, 4);
    }

    private void MovementInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _path = findWayInValidCells.Take() == null ? new List<Cell>() : findWayInValidCells.Take();
        }

        if (_path.Count != 0)
        {
            _inputIsPossible = false;

            unit.movable.Move(_path[0]);
            _path.RemoveAt(0);
        }
        else
        {
            pathVisualizer.ApplyEffect();
            movementCellsHighlighter.ApplyEffect();
        }
    }

    private void AttackInput()
    {

    }

    private void PushInput()
    {
        validForPushCellsTaker.Take().ForEach(cell => { cell.EnableValidateCellHighlight(false); cell.EnableSelectedCellHighlight(false); });
        pushCellsHighlighter.ApplyEffect();
        
        if(cellByMousePosition.Take() == null)
            return;
        
        if (validForPushCellsTaker.Take().Contains(cellByMousePosition.Take()[0]))
        {
            cellByMousePosition.Take()[0].EnableSelectedCellHighlight(true);
            
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                unit.pusher.ApplyEffect();
            }
        }
    }
}

public enum InputType
{
    movement = 0, attack = 1, push = 2, research = 3
}
