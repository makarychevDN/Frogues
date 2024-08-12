using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class GeneratedFloor : BaseFloor
    {
        [Header("Setup")]
        [SerializeField] private int expectedCountOfRooms = 16;
        [SerializeField] private int maximumOfNeigborsForEachButton = 4;
        [SerializeField] private float distanceBetweenButtonsMultiplier = 3;
        [Header("Links")]
        [SerializeField] private FloorGridGenerator floorGreedGenerator;
        [SerializeField] private Transform map;
        [SerializeField] private RoomButton roomButtonPrefab;
        [SerializeField] private TrailBetweenRoomButtons trailBetweenRoomButtonsPrefab;
        [SerializeField] private List<Room> roomPrefabs;
        [SerializeField] private SerializedDictionary<RoomButton, List<RoomButton>> buttonsAndTheirNeighborButtons = new();
        [SerializeField] private Sprite visitedRoomSprite;
        private List<RoomButton> _visitedRooms = new();
        private List<RoomButton> _availableToVisitiongRooms = new();

        private int _xSizeOfSpriteMap = 600;
        private int _ySizeOfSpriteMap = 300;

        public override void Init()
        {
            floorGreedGenerator.GenerateFloorGrid();

            List<FloorGeneratorNode> nodesToSpawn;
            do
            {
                ResetGeneratorSetup();
                nodesToSpawn = GetNodesToSpawn(floorGreedGenerator.Nodes.GetFirst());
                RemoveExtraNeighborLinks(nodesToSpawn, maximumOfNeigborsForEachButton);
            }
            while
            (!GeneratedMapIsOk(_xSizeOfSpriteMap, _ySizeOfSpriteMap, nodesToSpawn, distanceBetweenButtonsMultiplier));

            GenerateRoomButtons(nodesToSpawn);
            UpdatePositionOfButtons(roomButtons, CalculateDeltaToPlaceGraphInTheMiddleOfMap());
            SpawnTrails(buttonsAndTheirNeighborButtons.ToDictionary(x => x.Key, x => x.Value));

            base.Init();
        }

        private void RemoveExtraNeighborLinks(List<FloorGeneratorNode> nodesToSpawn, int maximumOfNeighborLinks)
        {
            var extraNeighborsNode = GetNodeWithExtraNeighbors(nodesToSpawn, maximumOfNeighborLinks);

            while(extraNeighborsNode != null)
            {
                FloorGeneratorNode mostLinkedNeighbor = 
                    extraNeighborsNode.Neighbors.Values
                    .OrderByDescending(node => node.Neighbors.Count).FirstOrDefault();

                Color keyOfMostLinkedNeighbor = extraNeighborsNode.Neighbors.FirstOrDefault(x => x.Value == mostLinkedNeighbor).Key;
                Color keyOfextraNeighborsNode = mostLinkedNeighbor.Neighbors.FirstOrDefault(x => x.Value == extraNeighborsNode).Key;

                extraNeighborsNode.Neighbors.Remove(keyOfMostLinkedNeighbor);
                mostLinkedNeighbor.Neighbors.Remove(keyOfextraNeighborsNode);

                extraNeighborsNode = GetNodeWithExtraNeighbors(nodesToSpawn, maximumOfNeighborLinks);
            }
        }

        private FloorGeneratorNode GetNodeWithExtraNeighbors(List<FloorGeneratorNode> nodesToSpawn, int maximumOfNeighborLinks) 
            => nodesToSpawn.FirstOrDefault(node => node.Neighbors.Values
                .Where(neighbor => nodesToSpawn.Contains(neighbor)).Count() > maximumOfNeighborLinks);


        private void ResetGeneratorSetup()
        {
            floorGreedGenerator.Nodes.ForEach(node => node.AlreadyUsedToSpawnRoom = false);
            buttonsAndTheirNeighborButtons.Clear();
        }

        private void GenerateRoomButtons(List<FloorGeneratorNode> nodesToSpawn)
        {
            Dictionary<FloorGeneratorNode, RoomButton> nodesAndButtons = new Dictionary<FloorGeneratorNode, RoomButton>();

            foreach (var node in nodesToSpawn)
            {
                var spawnedButton = SpawnButton(roomButtonPrefab, map, node, distanceBetweenButtonsMultiplier);
                spawnedButton.SetSpriteAsRoomIsUnavailable();
                roomButtons.Add(spawnedButton);
                spawnedButton.AbleToClick = false;
                nodesAndButtons.Add(node, spawnedButton);
            }

            FindNeigborButtons(nodesAndButtons);
            var randomRoomButton = roomButtons.GetRandomElement();
            MakeRoomButtonAvailable(randomRoomButton);
            _availableToVisitiongRooms.Add(randomRoomButton);
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
            spawnedButton.Button.onClick.AddListener(() => VisitRoom(spawnedButton));
            return spawnedButton;
        }

        private void VisitRoom(RoomButton roomButton)
        {
            roomButton.AbleToClick = false;
            roomButton.SetSprite(visitedRoomSprite);
            _visitedRooms.Add(roomButton);
            _availableToVisitiongRooms.Remove(roomButton);
            buttonsAndTheirNeighborButtons[roomButton]
                .Where(roomButton => !_visitedRooms.Contains(roomButton) && !_availableToVisitiongRooms.Contains(roomButton)).ToList()
                .ForEach(button => _availableToVisitiongRooms.Add(button));

            var roomWithTheMainQuest = roomButton.GetRoom() as IAbleToHaveTheMainQuest;
            if(roomWithTheMainQuest != null)
            {
                DisableAbailableButtons(_availableToVisitiongRooms);
                roomWithTheMainQuest.GetMainQuestCompletedEvent().AddListener(() => EnableAbailableButtons(_availableToVisitiongRooms));
            }
            else
            {
                EnableAbailableButtons(_availableToVisitiongRooms);
            }
        }

        private void DisableAbailableButtons(List<RoomButton> roomButtons)
        {
            roomButtons.ForEach(roomButton => roomButton.AbleToClick = false);
        }

        private void EnableAbailableButtons(List<RoomButton> roomButtons)
        {
            roomButtons.ForEach(roomButton => MakeRoomButtonAvailable(roomButton));
        }

        private void MakeRoomButtonAvailable(RoomButton roomButton)
        {
            roomButton.AbleToClick = true;
            roomButton.SetSpriteAsRoomIsAvailable();
        }

        private List<FloorGeneratorNode> GetNodesToSpawn(FloorGeneratorNode startNode)
        {
            List<FloorGeneratorNode> childNodes = new List<FloorGeneratorNode>() { startNode };
            List<FloorGeneratorNode> selectedNodes = new List<FloorGeneratorNode>() { startNode };
            startNode.AlreadyUsedToSpawnRoom = true;

            while (selectedNodes.Count < expectedCountOfRooms)
            {
                var selectedNode = childNodes.GetRandomElement();
                List<FloorGeneratorNode> newNodes = SelectNeigbors(selectedNode, Random.Range(2, 3));

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
            List<FloorGeneratorNode> neighbors = selectedNode.Neighbors.Values
                .Where(neigbor => !neigbor.AlreadyUsedToSpawnRoom).ToList();

            if(Random.Range(0, 2) == 0)
            {
                var theMainNeigbor = neighbors.GetRandomElement();
                neighbors = neighbors.OrderBy(neigbor => neigbor.DistanceToOtherNode(theMainNeigbor)).ToList();
            }
            else
            {
                neighbors.Shuffle();
            }

            List<FloorGeneratorNode> neighborsToSpawn = new();
            neigborQuantity = Mathf.Clamp(neigborQuantity, 0, neighbors.Count);
            for (int i = 0; i < neigborQuantity; i++)
            {
                neighborsToSpawn.Add(neighbors[i]);
                neighbors[i].AlreadyUsedToSpawnRoom = true;
            }

            return neighborsToSpawn;
        }

        private bool GeneratedMapIsOk(int maxXPosition, int maxYPosition, List<FloorGeneratorNode> nodesToSpawn, float distanceMultiplier)
        {
            return SizeOfMapFitsToMapSprite(maxXPosition, maxYPosition, nodesToSpawn, distanceMultiplier)
                && EveryNodeIsReachable(nodesToSpawn);
        }

        private bool SizeOfMapFitsToMapSprite(int maxXPosition, int maxYPosition, List<FloorGeneratorNode> nodesToSpawn, float distanceMultiplier)
        {
            return (nodesToSpawn.Max(node => node.Coordinates.x) - nodesToSpawn.Min(node => node.Coordinates.x)) * distanceMultiplier < maxXPosition
                && (nodesToSpawn.Max(node => node.Coordinates.y) - nodesToSpawn.Min(node => node.Coordinates.y)) * distanceMultiplier < maxYPosition;
        }

        private bool EveryNodeIsReachable(List<FloorGeneratorNode> nodesToSpawn)
        {
            var startNode = nodesToSpawn.GetFirst();
            List<FloorGeneratorNode> reachableNodes = new List<FloorGeneratorNode>() { startNode };
            List<FloorGeneratorNode> parentNodes = new List<FloorGeneratorNode>() { startNode };
            List<FloorGeneratorNode> childrenNodes = new List<FloorGeneratorNode>();
            List<FloorGeneratorNode> packOfReachableNodes;

            while (parentNodes.Count != 0)
            {
                foreach(var parentNode in parentNodes)
                {
                    packOfReachableNodes = 
                        parentNode.Neighbors.Values
                        .Where(node => nodesToSpawn.Contains(node) 
                        && !reachableNodes.Contains(node)).ToList();

                    childrenNodes.AddRange(packOfReachableNodes);
                    reachableNodes.AddRange(packOfReachableNodes);
                }

                parentNodes = childrenNodes;
                childrenNodes = new List<FloorGeneratorNode>();
            }

            return reachableNodes.Count == nodesToSpawn.Count;
        }
    }
}