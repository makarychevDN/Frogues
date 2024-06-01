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


        public void Init(List<Unit> playableCharacters)
        {
            InitQueue(playableCharacters);
            ActivateNext();
        }


        public bool IsUnitCurrent(Unit unit)
        {
            return _currentNode.Unit == unit;
        }

        private void InitQueue(List<Unit> playableCharacters)
        {
            _unitsList = new CycledLinkedList();
            var roomUnitsAbleToAct = GetComponentsInChildren<Unit>().Where(x => x.ActionsInput != null).ToList();
            roomUnitsAbleToAct.Remove(roundCounterBeforePlayer);
            roomUnitsAbleToAct.Remove(roundCounterBeforeEnemies);
            _unitsList.Add(roundCounterBeforePlayer);
            _unitsList.Add(roundCounterBeforeEnemies);

            foreach (var unit in playableCharacters)
            {
                _unitsList.AddAfterTargetObject(roundCounterBeforePlayer, unit);
            }

            foreach (var unit in roomUnitsAbleToAct)
            {
                _unitsList.AddAfterTargetObject(roundCounterBeforeEnemies, unit);
            }

            _debugUnits = _unitsList.ToList();
            _currentNode = _unitsList.HeadNode;
        }

        public void ActForCurrentUnit()
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
            if(!_unitsList.Contains(unit))
                return;
            
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