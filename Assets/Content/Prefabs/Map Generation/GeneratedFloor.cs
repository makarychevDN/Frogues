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

        public override void Init()
        {
            floorGreedGenerator.GenerateFloorGrid();
            GenerateRooms();
            base.Init();
        }

        public void GenerateRooms()
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

            Vector3 centerPosition = CalculateCenterPositionOfAllSpawnedButtons() - Vector2.zero;
            foreach (var spawnedButton in roomButtons)
            {
                spawnedButton.transform.localPosition -= centerPosition;
            }

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

        private Vector2 CalculateCenterPositionOfAllSpawnedButtons()
        {
            var totalX = 0f;
            var totalY = 0f;
            foreach (var roomButton in roomButtons)
            {
                totalX += roomButton.transform.localPosition.x;
                totalY += roomButton.transform.localPosition.y;
            }
            var centerX = totalX / roomButtons.Count;
            var centerY = totalY / roomButtons.Count;
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