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

    public CycledLinkedList(Unit mapObject)
    {
        InsertNodeInEmptyList(mapObject);
    }

    public CycledLinkedList(Unit[] mapObjects)
    {
        foreach (var item in mapObjects)
        {
            Add(item);
        }
    }

    public CycledLinkedList(List<Unit> mapObjects)
    {
        foreach (var item in mapObjects)
        {
            Add(item);
        }
    }
    #endregion

    #region AddMethods
    public void Add(Unit mapObject)
    {
        if (_headNode == null)
        {
            InsertNodeInEmptyList(mapObject);
        }

        else
        {
            AddToTheEndOfList(mapObject);
        }
    } 

    public void AddFirst(Unit mapObject)
    {
        if (_headNode == null)
        {
            InsertNodeInEmptyList(mapObject);
        }

        else
        {
            AddToTheEndOfList(mapObject);
            _headNode = _headNode.Previous;
        }
    }  

    public void AddSecond(Unit mapObject)
    {
        if (_headNode == null)
        {
            InsertNodeInEmptyList(mapObject);
        }

        else
        {
            InsertNode(new QueueNode(mapObject), _headNode, _headNode.Next);
        }
    }

    public void AddAfterTargetObject(Unit TargetObject, Unit newMapObject)
    {
        QueueNode temp = _headNode;
            
        while (temp.MapObject != TargetObject)
        {
            temp = temp.Next;
        }

        InsertNode(new QueueNode(newMapObject), temp, temp.Next);
    }
    
    public void AddBeforeTargetObject(Unit TargetObject, Unit newMapObject)
    {
        QueueNode temp = _headNode;
            
        while (temp.MapObject != TargetObject)
        {
            temp = temp.Next;
        }

        InsertNode(new QueueNode(newMapObject), temp.Previous, temp);
    }
    
    private void AddToTheEndOfList(Unit mapObject)
    {
        QueueNode temp = _headNode;
            
        while (temp.Next != _headNode)
        {
            temp = temp.Next;
        }
        
        InsertNode(new QueueNode(mapObject), temp, _headNode);        
    } 

    private void InsertNode(QueueNode newNode, QueueNode previous, QueueNode next)
    {
        newNode.Next = next;
        newNode.Previous = previous;
        previous.Next = newNode;
        next.Previous = newNode;
    }

    private void InsertNodeInEmptyList(Unit mapObject)
    {
        _headNode = new QueueNode(mapObject);
        _headNode.Next = _headNode;
        _headNode.Previous = _headNode;
    }
    #endregion

    #region RemoveMethods
    public void Remove(Unit mapObject)
    {
        if (_headNode == null)
            return;

        QueueNode temp = _headNode;
        
        while (temp.MapObject != mapObject)
        {
            temp = temp.Next;
        }

        temp.Previous.Next = temp.Next;
        temp.Next.Previous = temp.Previous;
        
        if (_headNode.MapObject == mapObject)
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


    public Unit MapObject
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

    public QueueNode(Unit mapObject)
    {
        this._unit = mapObject;
    }
}