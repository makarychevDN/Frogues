using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace FroguesFramework
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private bool isPeaceful;
        [SerializeField] private Vector2Int PositionOfCenterTile;

        [SerializeField] private Tile wallTile;
        [SerializeField] private List<Cell> cellsPrefabs;
        [SerializeField] private List<Cell> wallsPrefabs;
        [SerializeField] private Transform cellsParent;
        [SerializeField] private Transform wallsParent;
        [SerializeField] private List<Cell> allCells;
        [SerializeField] private List<Cell> walls;

        [Header("Links")]
        [SerializeField] private Tilemap localTilemap;
        [SerializeField] private UnitsQueue unitsQueue;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private Unit metaPlayer;
        [SerializeField] private UnitAndStartPosition player;
        [SerializeField] private BaseTrainingModificator trainingModificator;
        private Cell _exitCell;

        public UnitsQueue UnitsQueue => unitsQueue;
        public CameraController CameraController => cameraController;
        public Tilemap LocalTilemap => localTilemap;
        public bool IsPeaceful => isPeaceful;
        public List<Cell> AllCells => allCells;
        public List<Cell> Walls => walls;
        
        public Vector3 CenterOfRoom => cameraController.transform.position;
        public UnityEvent onRoomInited;

        public Vector3 GetDeltaOfCenterPosition()
        {
            return localTilemap.CellToWorld(new Vector3Int(PositionOfCenterTile.x, PositionOfCenterTile.y)) - transform.position;
        }

        public void Init()
        {
            /*cameraController.Init();
            InitPlayer();
            unitsQueue.Player = player.unit;

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

            onRoomInited.Invoke();*/
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

        public void TurnOffTileMapRenderer()
        {
            localTilemap.GetComponent<TilemapRenderer>().enabled = false;
        }

        [ContextMenu("Switch Tilemap Renderer Enabled")]
        public void SwitchTilemapRendererEnabled()
        {
            var tilemapRenderer = localTilemap.GetComponent<TilemapRenderer>();
            tilemapRenderer.enabled = !tilemapRenderer.enabled;
        }

        [ContextMenu("Destroy All Cells")]
        public void DestroyAllCells()
        {
            for (int i = 0; i < allCells.Count; i++)
            {
                DestroyImmediate(allCells[i].gameObject);
            }
            allCells.Clear();

            for (int i = 0; i < walls.Count; i++)
            {
                DestroyImmediate(walls[i].gameObject);
            }
            walls.Clear();
        }

        [ContextMenu("Clamp Position To Global Tile Map")]
        public void ClampToGlobalTileMap()
        {
            Floor floor = GetComponentInParent<Floor>();
            if (floor == null)
            {
                Debug.LogError("There is no Floor component in parents");
                return;
            }

            Tilemap globalTilemap = floor.GlobalTilemap;
            if (globalTilemap == null)
            {
                Debug.LogError("There is no Global Tilemap in the floor");
                return;
            }

            transform.position = globalTilemap.CellToWorld(globalTilemap.WorldToCell(transform.position));
        }

        [ContextMenu("Generate Cells By Local Tilemap")]
        public void GenerateCellsByLocalTilemap()
        {
            DestroyAllCells();
            localTilemap.CompressBounds();
            BoundsInt bounds = localTilemap.cellBounds;
            TileBase[] allTiles = localTilemap.GetTilesBlock(bounds);
            var localCellsArray = new Cell[bounds.size.x, bounds.size.y];

            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    TileBase tile = Extensions.GetTileFromListByCoordinates(allTiles, bounds, x, y);

                    if (tile != null)
                    {
                        Cell spawnedCell;

                        if (tile == wallTile)
                        {
                            spawnedCell = Instantiate(wallsPrefabs.GetRandomElement(), wallsParent);
                            walls.Add(spawnedCell);
                        }
                        else
                        {
                            spawnedCell = Instantiate(cellsPrefabs.GetRandomElement(), cellsParent);
                            allCells.Add(spawnedCell);
                        }

                        localCellsArray[x, y] = spawnedCell;
                        spawnedCell.Coordinates = new Vector2Int(x, y);
                        spawnedCell.transform.position = localTilemap.CellToWorld(new Vector3Int(x, y));
                    }
                }
            }
        }

        [Serializable]
        public struct UnitAndStartPosition
        {
            public Unit unit;
            public Vector2Int startPosition;
        }
    }
}
