using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycledLinkedList : IEnumerable
{
    private QueueNode _headNode;

    public QueueNode HeadNode
    {
        get => _headNode;
        set => _headNode = value;
    }

    #region Constructors
    public CycledLinkedList()
    {
        _headNode = null;
    }

    public CycledLinkedList(Unit unit)
    {
        InsertNodeInEmptyList(unit);
    }

    public CycledLinkedList(Unit[] units)
    {
        foreach (var item in units)
        {
            Add(item);
        }
    }

    public CycledLinkedList(List<Unit> units)
    {
        foreach (var item in units)
        {
            Add(item);
        }
    }
    #endregion

    #region AddMethods
    public void Add(Unit unit)
    {
        if (_headNode == null)
        {
            InsertNodeInEmptyList(unit);
        }

        else
        {
            AddToTheEndOfList(unit);
        }
    } 

    public void AddFirst(Unit unit)
    {
        if (_headNode == null)
        {
            InsertNodeInEmptyList(unit);
        }

        else
        {
            AddToTheEndOfList(unit);
            _headNode = _headNode.Previous;
        }
    }  

    public void AddSecond(Unit unit)
    {
        if (_headNode == null)
        {
            InsertNodeInEmptyList(unit);
        }

        else
        {
            InsertNode(new QueueNode(unit), _headNode, _headNode.Next);
        }
    }

    public void AddAfterTargetObject(Unit TargetObject, Unit unit)
    {
        QueueNode temp = _headNode;
            
        while (temp.Unit != TargetObject)
        {
            temp = temp.Next;
        }

        InsertNode(new QueueNode(unit), temp, temp.Next);
    }
    
    public void AddBeforeTargetObject(Unit TargetObject, Unit unit)
    {
        QueueNode temp = _headNode;
            
        while (temp.Unit != TargetObject)
        {
            temp = temp.Next;
        }

        InsertNode(new QueueNode(unit), temp.Previous, temp);
    }
    
    private void AddToTheEndOfList(Unit unit)
    {
        QueueNode temp = _headNode;
            
        while (temp.Next != _headNode)
        {
            temp = temp.Next;
        }
        
        InsertNode(new QueueNode(unit), temp, _headNode);        
    } 

    private void InsertNode(QueueNode newNode, QueueNode previous, QueueNode next)
    {
        newNode.Next = next;
        newNode.Previous = previous;
        previous.Next = newNode;
        next.Previous = newNode;
    }

    private void InsertNodeInEmptyList(Unit unit)
    {
        _headNode = new QueueNode(unit);
        _headNode.Next = _headNode;
        _headNode.Previous = _headNode;
    }
    #endregion

    #region RemoveMethods
    public void Remove(Unit unit)
    {
        if (_headNode == null)
            return;

        QueueNode temp = _headNode;
        
        while (temp.Unit != unit)
        {
            temp = temp.Next;
        }

        temp.Previous.Next = temp.Next;
        temp.Next.Previous = temp.Previous;
        
        if (_headNode.Unit == unit)
        {
            _headNode = temp.Next;
        }
    }
    #endregion

    #region InterfaceImplementationStaff
    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
    
    public CycledQueueEnum GetEnumerator()
    {
        return new CycledQueueEnum(_headNode);
    }
    #endregion
}

public class CycledQueueEnum : IEnumerator
{
    public QueueNode _head;
    public QueueNode _current;
    public CycledQueueEnum(QueueNode node)
    {
        _head = node;
    }

    public bool MoveNext()
    {
        if (_head == null)
        {
            return false;
        }
        
        if (_current == null)
        {
            _current = _head;
            return true;
        }
        
        _current = _current.Next;
        return (_current != _head);
    }

    public void Reset()
    {
        Current = _head;
    }

    object IEnumerator.Current
    {
        get
        {
            return Current;
        }
    }

    public QueueNode Current
    {
        get
        {
            return _current;
        }

        set
        {
            _current = value;
        }
    }
}

public class QueueNode
{
    private Unit _unit;
    private QueueNode _next;
    private QueueNode _previous;


    public Unit Unit
    {
        get => _unit;
        set => _unit = value;
    }

    public QueueNode Next
    {
        get => _next;
        set => _next = value;
    }
    
    public QueueNode Previous
    {
        get => _previous;
        set => _previous = value;
    }

    public QueueNode(Unit unit)
    {
        this._unit = unit;
    }
}