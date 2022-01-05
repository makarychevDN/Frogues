using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class UnitsQueue : MonoBehaviour
{
    public static UnitsQueue Instance;
    [SerializeField] private Unit player;
    [SerializeField] private Unit roundCounter;
    [SerializeField] private bool playerDied;

    private CycledLinkedList _unitsList;
    private QueueNode _currentNode;
    [SerializeField, ReadOnly] private Unit _debugCurrentUnit;
    [SerializeField, ReadOnly] private List<Unit> _debugUnits;

    public int Count => _unitsList.Count;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        InitQueue();
        ActivateNext();
    }

    public bool IsUnitCurrent(Unit unit)
    {
        return _currentNode.Unit == unit;
    }

    public void InitQueue()
    {
        _unitsList = new CycledLinkedList();

        var actUnits = FindObjectsOfType<Unit>().Where(x => x.input != null).ToList();

        foreach(var unit in actUnits)
        {
        
            if (unit == player || unit == roundCounter)
                continue;

            else if(unit.unitType == MapLayer.Surface)
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
        if (CurrentlyActiveObjects.SomethingIsActNow || playerDied)
            return;
            
        _currentNode.Unit.input.Act();
    }

    public void ActivateNext()
    {
        if (Count <= 1)
            return;

        _currentNode = _currentNode.Next;
        _debugCurrentUnit = _currentNode.Unit;
    }

    public void Remove(Unit unit)
    {
        if (_currentNode.Unit == unit)
            ActivateNext();

        if (unit == player)
            playerDied = true;

        _unitsList.Remove(unit);
        _debugUnits.Remove(unit);
    }

    public void AddObjectInQueue(Unit unit)
    {
        _unitsList.Add(unit);
        _debugUnits.Add(unit);
    }

    public void AddObjectInQueueAfterPlayer(Unit unit)
    {
        _unitsList.AddSecond(unit);
        _debugUnits.Insert(1, unit);
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
