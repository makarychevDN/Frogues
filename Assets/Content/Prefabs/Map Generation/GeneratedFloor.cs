using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class GeneratedFloor : BaseFloor
    {
        [SerializeField] private FloorGridGenerator floorGreedGenerator;
        [SerializeField] private Transform map;
        [SerializeField] private RoomButton roomButtonPrefab;
        [SerializeField] private int expectedCountOfRooms = 16;
        [SerializeField] private TrailBetweenRoomButtons trailBetweenRoomButtonsPrefab;
        [SerializeField] private float distanceBetweenButtonsMultiplier = 3;
        [SerializeField] private List<Room> roomPrefabs;
        private int _xSizeOfSpriteMap = 300;
        private int _ySizeOfSpriteMap = 300;
        [SerializeField] private SerializedDictionary<RoomButton, List<RoomButton>> buttonsAndTheirNeighborButtons = new();

        public override void Init()
        {
            floorGreedGenerator.GenerateFloorGrid();

            List<FloorGeneratorNode> nodesToSpawn;
            do
            {
                ResetGeneratorSetup();
                RemoveExistingButtonsAndTrails();
                nodesToSpawn = GetNodesToSpawn(floorGreedGenerator.Nodes.GetRandomElement());
            } 
            while (!GeneratedRoomIsOk(_xSizeOfSpriteMap, _ySizeOfSpriteMap, nodesToSpawn, distanceBetweenButtonsMultiplier));

            GenerateRoomButtons(nodesToSpawn);
            UpdatePositionOfButtons(roomButtons, CalculateDeltaToPlaceGraphInTheMiddleOfMap());
            SpawnTrails(buttonsAndTheirNeighborButtons.ToDictionary(x => x.Key, x => x.Value));

            base.Init();
        }

        private void ResetGeneratorSetup()
        {
            floorGreedGenerator.Nodes.ForEach(node => node.AlreadyUsedToSpawnRoom = false);
            buttonsAndTheirNeighborButtons.Clear();
        }

        private void RemoveExistingButtonsAndTrails()
        {
            var allChildren = map.GetComponentsInChildren<Transform>().ToList();
            allChildren.Remove(map);
            for (var i = 0; i < allChildren.Count(); i++)
            {
                Destroy(allChildren[i].gameObject);
            }

            roomButtons.Clear();
        }

        private void GenerateRoomButtons(List<FloorGeneratorNode> nodesToSpawn)
        {
            Dictionary<FloorGeneratorNode, RoomButton> nodesAndButtons = new Dictionary<FloorGeneratorNode, RoomButton>();

            foreach (var node in nodesToSpawn)
            {
                var spawnedButton = SpawnButton(roomButtonPrefab, map, node, distanceBetweenButtonsMultiplier);
                roomButtons.Add(spawnedButton);
                spawnedButton.AbleToClick = false;
                nodesAndButtons.Add(node, spawnedButton);
            }

            FindNeigborButtons(nodesAndButtons);
            roomButtons.GetRandomElement().AbleToClick = true;
        }

        private void FindNeigborButtons(Dictionary<FloorGeneratorNode, RoomButton> nodesAndButtons)
        {
            foreach(var nodeAndButton in nodesAndButtons)
            {
                var neighborButtons = new List<RoomButton>();
                buttonsAndTheirNeighborButtons.Add(nodeAndButton.Value, neighborButtons);

                foreach(var nodeNeighbor in nodeAndButton.Key.Neighbors.Values.Where(node => nodesAndButtons.Keys.Contains(node)))
                {
                    neighborButtons.Add(nodesAndButtons[nodeNeighbor]);
                }
            }
        }

        private void SpawnTrails(Dictionary<RoomButton, List<RoomButton>> buttonsAndTheirNeighborButtons)
        {
            List<TrailBetweenRoomButtons> trailBetweenRoomButtons = new List<TrailBetweenRoomButtons>();

            foreach(var buttonAndNeighbors in buttonsAndTheirNeighborButtons)
            {
                foreach (var neighbor in buttonAndNeighbors.Value)
                {
                    if (trailBetweenRoomButtons.ContainsTheSameTrail(buttonAndNeighbors.Key, neighbor))
                        continue;

                    var spawnedTrail = Instantiate(trailBetweenRoomButtonsPrefab, map);
                    spawnedTrail.Init(buttonAndNeighbors.Key, neighbor);
                    spawnedTrail.UpdateTransform();
                    trailBetweenRoomButtons.Add(spawnedTrail);
                }
            }
        }

        private void UpdatePositionOfButtons(List<RoomButton> roomButtons, Vector2 offset)
        {
            foreach (var roomButton in roomButtons)
            {
                roomButton.transform.localPosition -= offset.ToVector3();
            }
        }

        private Vector2 CalculateDeltaToPlaceGraphInTheMiddleOfMap()
        {
            float centerX = (roomButtons.Max(roomButton => roomButton.transform.localPosition.x) +
                roomButtons.Min(roomButton => roomButton.transform.localPosition.x)) * 0.5f;

            float centerY = (roomButtons.Max(roomButton => roomButton.transform.localPosition.y) +
                roomButtons.Min(roomButton => roomButton.transform.localPosition.y)) * 0.5f;

            return new Vector2(centerX, centerY);
        }

        private RoomButton SpawnButton(RoomButton roomButtonPrefab, Transform parent, FloorGeneratorNode node, float distanceMultiplier)
        {
            var spawnedButton = Instantiate(roomButtonPrefab, parent);
            spawnedButton.transform.localPosition -= node.Coordinates.ToVector3() * distanceMultiplier;
            spawnedButton.Init(roomPrefabs.GetRandomElement());
            //spawnedRoom.OnRoomWasEnabled.AddListener(spawnedButton.SetButtonIsInteractable);
            return spawnedButton;
        }

        private List<FloorGeneratorNode> GetNodesToSpawn(FloorGeneratorNode startNode)
        {
            List<FloorGeneratorNode> childNodes = new List<FloorGeneratorNode>() { startNode };
            List<FloorGeneratorNode> selectedNodes = new List<FloorGeneratorNode>() { startNode };
            startNode.AlreadyUsedToSpawnRoom = true;

            while (selectedNodes.Count < expectedCountOfRooms - 1)
            {
                var selectedNode = childNodes.GetRandomElement();
                List<FloorGeneratorNode> newNodes = SelectNeigbors(selectedNode, Random.Range(1, 3));

                foreach (var newNode in newNodes)
                {
                    selectedNodes.Add(newNode);
                }

                childNodes.Remove(selectedNode);
                childNodes.AddRange(newNodes);
            }

            return selectedNodes;
        }

        private List<FloorGeneratorNode> SelectNeigbors(FloorGeneratorNode selectedNode, int neigborQuantity)
        {
            List<FloorGeneratorNode> neighbors = selectedNode.Neighbors.Values.ToList();
            var theMainNeigbor = neighbors.GetRandomElement();
            neighbors = neighbors.OrderBy(neigbor => neigbor.DistanceToOtherNode(theMainNeigbor))
                .Where(neigbor => !neigbor.AlreadyUsedToSpawnRoom).ToList();

            List<FloorGeneratorNode> neighborsToSpawn = new();
            neigborQuantity = Mathf.Clamp(neigborQuantity, 0, neighbors.Count);
            for (int i = 0; i < neigborQuantity; i++)
            {
                neighborsToSpawn.Add(neighbors[i]);
                neighbors[i].AlreadyUsedToSpawnRoom = true;
            }

            return neighborsToSpawn;
        }

        private bool GeneratedRoomIsOk(int maxXPosition, int maxYPosition, List<FloorGeneratorNode> nodesToSpawn, float distanceMultiplier)
        {
            return (nodesToSpawn.Max(node => node.Coordinates.x) - nodesToSpawn.Min(node => node.Coordinates.x)) * distanceMultiplier < maxXPosition
                && (nodesToSpawn.Max(node => node.Coordinates.y) - nodesToSpawn.Min(node => node.Coordinates.y)) * distanceMultiplier < maxYPosition;
        }

        private void AddListenersToRoomButtons()
        {
            foreach(var buttonAndNeighbors in buttonsAndTheirNeighborButtons)
            {
                buttonAndNeighbors.Key.OnRoomButtonSelected.AddListener(() => MakeButtonsAbleToClick(buttonAndNeighbors.Value));
            }
        }

        private void MakeButtonsAbleToClick(List<RoomButton> roomButtons)
        {
            roomButtons.ForEach(roomButton => roomButton.AbleToClick = true);
        }
    }
}