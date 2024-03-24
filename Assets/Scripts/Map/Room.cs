using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Map map;
        [SerializeField] private bool isPeaceful;
        [SerializeField] private Vector2Int exitPosition;
        [SerializeField] private PathFinder pathFinder;
        [SerializeField] private UnitsQueue unitsQueue;
        [SerializeField] private CameraController cameraController;

        [SerializeField] private Unit metaPlayer;
        [SerializeField] private UnitAndStartPosition player;
        [SerializeField] private List<UnitAndStartPosition> unitsAndStartPositions;

        [SerializeField] private BaseTrainingModificator trainingModificator;
        private Cell _exitCell;

        public PathFinder PathFinder => pathFinder;
        public Map Map => map;
        public UnitsQueue UnitsQueue => unitsQueue;
        public CameraController CameraController => cameraController;
        public bool IsPeaceful => isPeaceful;
        
        public Vector3 CenterOfRoom => cameraController.transform.position;
        public UnityEvent onRoomInited;

        public void Init()
        {
            map.Init();
            PutUnitsOnCells();
            cameraController.Init();
            pathFinder.Init();
            InitPlayer();
            unitsQueue.Player = player.unit;

            _exitCell = map.GetCell(exitPosition);
            if(_exitCell != null)
                _exitCell.OnBecameFullByUnit.AddListener(TryToActivateNextRoom);

            foreach (var ableToAct in FindObjectsOfType<MonoBehaviour>().OfType<IAbleToAct>())
            {
                ableToAct.Init();
            }
            
            foreach (var unit in FindObjectsOfType<Unit>())
            {
                unit.Init();
            }
            
            unitsQueue.Init();

            if(trainingModificator != null)
                trainingModificator.Init();

            onRoomInited.Invoke();
        }

        public void PutUnitsOnCells()
        {
            PutUnitOnCell(player.unit, map.GetCell(player.startPosition));

            foreach(var unitAndStartPosition in unitsAndStartPositions)
            {
                PutUnitOnCell(unitAndStartPosition.unit, map.GetCell(unitAndStartPosition.startPosition));
            }
        }

        private void PutUnitOnCell(Unit unit, Cell cell)
        {
            cell.Content = unit;
            unit.CurrentCell = cell;
            unit.transform.position = cell.transform.position;
        }
        
        public void Init(Unit metaPlayer)
        {
            this.metaPlayer = metaPlayer;
            Init();
        }

        private void InitPlayer()
        {
            if (metaPlayer == null)
            {
                metaPlayer = player.unit;
            }
            
            var playerInstance = metaPlayer;
            player.unit.CurrentCell.Content = playerInstance;
            playerInstance.CurrentCell = player.unit.CurrentCell;
            playerInstance.transform.position = player.unit.transform.position;
            player.unit.gameObject.SetActive(false);
            player.unit = playerInstance;
        }

        public void ActivateExit()
        {
            if (_exitCell.gameObject.activeSelf)
                return;

            _exitCell.gameObject.SetActive(true);
            _exitCell.Content = null;
            _exitCell.OnBecameFullByUnit.AddListener(TryToActivateNextRoom);
        }

        private void TryToActivateNextRoom(Unit unit)
        {
            if(unit == metaPlayer)
            {
                EntryPoint.Instance.StartNextRoom();
            }
        }

        public void Deactivate()
        {
            cameraController.Deactivate();
            GetComponentsInChildren<Cell>().ToList().ForEach(cell => EntryPoint.Instance.RemoveAbleToDisablePreVisualizationToCollection(cell));
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        [Serializable]
        public struct UnitAndStartPosition
        {
            public Unit unit;
            public Vector2Int startPosition;
        }
    }
}
