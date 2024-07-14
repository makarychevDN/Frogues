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

        public override void Init()
        {
            floorGreedGenerator.GenerateFloorGrid();

            do
            {
                ResetFloorGrid();
                RemoveExistingButtonsAndTrails();
                GenerateRoomButtons();
            }
            while (!GeneratedRoomIsOk());

            base.Init();
        }

        private bool GeneratedRoomIsOk()
        {
            return roomButtons.Max(roomButton => roomButton.transform.position.x) - roomButtons.Min(roomButton => roomButton.transform.position.x) < 600
                && roomButtons.Max(roomButton => roomButton.transform.position.y) - roomButtons.Min(roomButton => roomButton.transform.position.y) < 600;
        }

        private void ResetFloorGrid()
        {
            floorGreedGenerator.Nodes.ForEach(node => node.AlreadyUsedToSpawnRoom = false);
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

        public void GenerateRoomButtons()
        {
            var startNode = floorGreedGenerator.Nodes.GetRandomElement();
            startNode.AlreadyUsedToSpawnRoom = true;
            List<FloorGeneratorNode> childNodes = new List<FloorGeneratorNode>() { startNode };
            Dictionary<FloorGeneratorNode, RoomButton> nodesToSpawn = new() { { startNode, SpawnRoomButton(startNode, distanceBetweenButtonsMultiplier) } };

            while (nodesToSpawn.Count < expectedCountOfRooms - 1)
            {
                var selectedNode = childNodes.GetRandomElement();
                List<FloorGeneratorNode> newNodes = SelectNeigbors(selectedNode, Random.Range(1, 3));

                foreach (var newNode in newNodes)
                {
                    nodesToSpawn.Add(newNode, SpawnRoomButton(newNode, distanceBetweenButtonsMultiplier));
                }

                childNodes.Remove(selectedNode);
                childNodes.AddRange(newNodes);
            }

            Vector3 deltaVector = CalculateDeltaToPlaceGraphInTheMiddleOfMap() - Vector2.zero;
            foreach (var spawnedButton in roomButtons)
            {
                spawnedButton.transform.localPosition -= deltaVector;
                spawnedButton.AbleToClick = false;

                var spawnedRoom = Instantiate(roomPrefabs.GetRandomElement());
                spawnedRoom.gameObject.SetActive(false);
                spawnedRoom.OnRoomWasEnabled.AddListener(spawnedButton.SetButtonIsInteractable);
                spawnedButton.Init(spawnedRoom);
            }
            roomButtons[0].AbleToClick = true;

            GenerateTrailsBetweenButtons(nodesToSpawn);
        }

        public void GenerateTrailsBetweenButtons(Dictionary<FloorGeneratorNode, RoomButton> nodesAndButtons)
        {
            List<TrailBetweenRoomButtons> spawnedTrails = new();

            foreach (var nodeAndButton in nodesAndButtons)
            {
                foreach (var neighbor in nodeAndButton.Key.Neighbors.Values)
                {
                    if (nodesAndButtons.ContainsKey(neighbor))
                    {
                        var spawnedTrail = Instantiate(trailBetweenRoomButtonsPrefab, map);
                        var targetRoomButton = nodesAndButtons.First(nodeAndButton => nodeAndButton.Key == neighbor).Value;
                        spawnedTrail.Init(nodeAndButton.Value, targetRoomButton);
                        spawnedTrail.UpdateTransform();
                        spawnedTrails.Add(spawnedTrail);
                    }
                }
            }

            for (int i = 0; i < spawnedTrails.Count; i++)
            {
                if (spawnedTrails[i].EqualToOtherLineInTheList(spawnedTrails))
                {
                    var spawnedTrail = spawnedTrails[i];
                    spawnedTrails.Remove(spawnedTrails[i]);
                    Destroy(spawnedTrail.gameObject);
                    i--;
                }
            }
        }

        private Vector2 CalculateDeltaToPlaceGraphInTheMiddleOfMap()
        {
            float centerX = (roomButtons.Max(roomButton => roomButton.transform.localPosition.x) +
                roomButtons.Min(roomButton => roomButton.transform.localPosition.x)) * 0.5f;

            float centerY = (roomButtons.Max(roomButton => roomButton.transform.localPosition.y) +
                roomButtons.Min(roomButton => roomButton.transform.localPosition.y)) * 0.5f;

            return new Vector2 (centerX, centerY);
        }

        private RoomButton SpawnRoomButton(FloorGeneratorNode selectedNode, float distanceBetweenButtonsMultiplier)
        {
            selectedNode.AlreadyUsedToSpawnRoom = true;
            var spawnedRoomButton = Instantiate(roomButtonPrefab, map);
            spawnedRoomButton.transform.localPosition = new Vector3(selectedNode.Coordinates.x, selectedNode.Coordinates.y) * distanceBetweenButtonsMultiplier;
            spawnedRoomButton.FloorGeneratorNode = selectedNode;
            roomButtons.Add(spawnedRoomButton);
            return spawnedRoomButton;
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
    }
}