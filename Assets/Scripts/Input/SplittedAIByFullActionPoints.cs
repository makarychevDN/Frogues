using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplittedAIByFullActionPoints : BaseInput
{
    [SerializeField] private BaseInput inputOnFullActionPoints;
    [SerializeField] private BaseInput inputOnNonFullActionPoints;
    [SerializeField] private ActionPoints actionPoints;
    private BaseInput _currentInput;

    private void Start()
    {
        inputOnFullActionPoints.OnInputDone.AddListener(EndInput);
        inputOnNonFullActionPoints.OnInputDone.AddListener(EndInput);
        actionPoints.OnActionPointsEnded.AddListener(EndInput);
    }

    public override void Act()
    {
        print("action points fullnes is " + actionPoints.Full);
        if(_currentInput == null)
        {
            print("im choosing input type");
            _currentInput = actionPoints.Full ? inputOnFullActionPoints : inputOnNonFullActionPoints;
        }

        print("current behaviour is " + _currentInput.gameObject.name);

        _currentInput.Act();
    }

    public void EndInput()
    {
        print("input ended");
        _currentInput = null;
    }
}
