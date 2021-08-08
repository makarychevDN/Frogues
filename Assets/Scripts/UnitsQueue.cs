using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class UnitsQueue : MonoBehaviour
{
    public static UnitsQueue Instance;
    private CycledLinkedList _linkedList;
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
        _linkedList = new CycledLinkedList();

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
                _linkedList.AddFirst(unit);
                _debugUnits.Insert(0, unit);
            }
            else
            {
                _linkedList.Add(unit);
                _debugUnits.Add(unit);
            }
        }

        _linkedList.AddFirst(player);
        _debugUnits.Insert(0, player);

        _linkedList.AddFirst(roundCounter);
        _debugUnits.Insert(0, roundCounter);

        _currentNode = _linkedList.HeadNode;
        _currentNode.Unit._input._inputIsPossible = true;
    }

    public void ActivateNext()
    {
        _currentNode.Unit._input._inputIsPossible = false;
        _currentNode = _currentNode.Next;
        _currentNode.Unit._input._inputIsPossible = true;
    }
}
