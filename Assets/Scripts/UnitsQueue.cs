using System.Collections.Generic;
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
        private Room _room;
        [SerializeField, ReadOnly] private Unit _debugCurrentUnit;
        [SerializeField, ReadOnly] private List<Unit> _debugUnits;

        public int Count => _unitsList.Count;

        public Unit Player
        {
            set => player = value;
        }


        public void Init(Room room, List<Unit> playableCharacters, List<Unit> otherAbleToActCharacters)
        {
            _room = room;
            InitQueue(playableCharacters, otherAbleToActCharacters);
            ActivateNext();
        }


        public bool IsUnitCurrent(Unit unit)
        {
            return _currentNode.Unit == unit;
        }

        private void InitQueue(List<Unit> playableCharacters, List<Unit> otherAbleToActCharacters)
        {
            otherAbleToActCharacters.Remove(roundCounterBeforePlayer);
            otherAbleToActCharacters.Remove(roundCounterBeforeEnemies);
            _unitsList = new CycledLinkedList
            {
                roundCounterBeforePlayer,
                roundCounterBeforeEnemies
            };

            foreach (var unit in playableCharacters)
            {
                _unitsList.AddAfterTargetObject(roundCounterBeforePlayer, unit);
            }

            foreach (var unit in otherAbleToActCharacters)
            {
                _unitsList.AddAfterTargetObject(roundCounterBeforeEnemies, unit);
            }

            _debugUnits = _unitsList.ToList();
            _currentNode = _unitsList.HeadNode;
        }

        public void ActForCurrentUnit()
        {
            if (_room.CurrentlyActiveObjects.SomethingIsActNow || playerDied)
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