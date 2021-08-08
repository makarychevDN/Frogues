using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class UnitsQueue : MonoBehaviour
{
    public static UnitsQueue Instance;
    private CycledLinkedList _unitsList;
    private QueueNode _currentNode;

    [SerializeField, ReadOnly] private List<Unit> _debugUnits;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        AddAllUnitsInQueue();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ActivateNext();
        }
    }

    public void AddAllUnitsInQueue()
    {
        _unitsList = new CycledLinkedList();

        var actUnits = FindObjectsOfType<Unit>().Where(x => x.GetComponentInChildren<ActionPoints>() != null).ToList();
        Unit player = null;
        Unit roundCounter = null;

        foreach(var unit in actUnits)
        {
            if(unit.name == "Player")
            {
                player = unit;
            }
            else if (unit.name == "Round Counter Unit")
            {
                roundCounter = unit;
            }
            else if(unit._unitType == MapLayer.Surface)
            {
                _unitsList.AddFirst(unit);
                _debugUnits.Insert(0, unit);
            }
            else
            {
                _unitsList.Add(unit);
                _debugUnits.Add(unit);
            }
        }

        _unitsList.AddFirst(player);
        _debugUnits.Insert(0, player);

        _unitsList.AddFirst(roundCounter);
        _debugUnits.Insert(0, roundCounter);

        _currentNode = _unitsList.HeadNode;
        _currentNode.Unit._input._inputIsPossible = true;
    }

    public void ActivateNext()
    {
        _currentNode.Unit._input._inputIsPossible = false;
        _currentNode = _currentNode.Next;
        _currentNode.Unit._input._inputIsPossible = true;
    }

    public void Remove(Unit unit)
    {
        if (_currentNode.Unit == unit)
            ActivateNext();

        _unitsList.Remove(unit);
    }
}
