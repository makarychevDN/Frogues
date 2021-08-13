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

    public int Count => _unitsList.Count;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        InitQueue();
        ActivateNext();
    }

    public void InitQueue()
    {
        _unitsList = new CycledLinkedList();

        var actUnits = FindObjectsOfType<Unit>().Where(x => x.GetComponentInChildren<BaseInput>() != null).ToList();
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
    }

    private void Update()
    {
        if (!CurrentlyActiveObjects.SomethingIsActNow)
            _currentNode.Unit._input.Act();
    }

    public void ActivateNext()
    {
        if (Count > 1)
            _currentNode = _currentNode.Next;
    }

    public void Remove(Unit unit)
    {
        if (_currentNode.Unit == unit)
            ActivateNext();

        _unitsList.Remove(unit);
        _debugUnits.Remove(unit);
    }

    public void AddObjectInQueue(Unit unit)
    {
        _unitsList.Add(unit);
    }
    public void AddObjectInQueueAfterPlayer(Unit unit)
    {
        _unitsList.AddSecond(unit);
    }

    public void AddObjectInQueueAfterTarget(Unit target, Unit unit)
    {
        _unitsList.AddAfterTargetObject(target, unit);
    }

    public void AddObjectInQueueBeforeTarget(Unit target, Unit unit)
    {
        _unitsList.AddBeforeTargetObject(target, unit);
    }
}
