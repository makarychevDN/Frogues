using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class UnitsQueue : MonoBehaviour
    {
        [SerializeField] private Unit player;
        [SerializeField] private Unit roundCounterBeforePlayer;
        [SerializeField] private Unit roundCounterBeforeEnemies;
        [SerializeField] private bool playerDied;
        public UnityEvent OnPlayerDied;

        private CycledLinkedList _unitsList;
        private QueueNode _currentNode;
        [SerializeField, ReadOnly] private Unit _debugCurrentUnit;
        [SerializeField, ReadOnly] private List<Unit> _debugUnits;

        public int Count => _unitsList.Count;

        public Unit Player
        {
            set => player = value;
        }


        public void Init()
        {
            InitQueue();
            ActivateNext();
        }


        public bool IsUnitCurrent(Unit unit)
        {
            return _currentNode.Unit == unit;
        }

        private void InitQueue()
        {
            _unitsList = new CycledLinkedList();

            var actUnits = FindObjectsOfType<Unit>().Where(x => x.ActionsInput != null).ToList();

            foreach (var unit in actUnits)
            {
                if (unit == player || unit == roundCounterBeforePlayer || unit == roundCounterBeforeEnemies)
                    continue;

                if (unit.unitType == MapLayer.Surface)
                {
                    _unitsList.AddFirst(unit);
                }
                else
                {
                    _unitsList.Add(unit);
                }
            }
            
            _unitsList.AddFirst(roundCounterBeforePlayer);
            _unitsList.AddAfterTargetObject(roundCounterBeforePlayer, player);
            _unitsList.AddAfterTargetObject(player, roundCounterBeforeEnemies);

            _debugUnits = _unitsList.ToList();
            _currentNode = _unitsList.HeadNode;
        }

        private void Update()
        {
            if (CurrentlyActiveObjects.SomethingIsActNow || playerDied)
                return;

            _currentNode.Unit.ActionsInput.Act();
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
            {
                playerDied = true;
                OnPlayerDied.Invoke();
            }

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
}