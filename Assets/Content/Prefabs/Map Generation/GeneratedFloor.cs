using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace FroguesFramework
{
    public class GeneratedFloor : BaseFloor
    {
        [SerializeField] private FloorGridGenerator floorGreedGenerator;
        [SerializeField] private Transform map;
        [SerializeField] private RoomButton roomButtonPrefab;
        [SerializeField] private TrailBetweenRoomButtons trailBetweenRoomButtonsPrefab;
        [SerializeField] private float distanceBetweenButtonsMultiplier = 3;

        public override void Init()
        {
            GenerateRooms();
            base.Init();
        }

        public void GenerateRooms()
        {
            floorGreedGenerator.GenerateFloorGrid();

            var startNode = floorGreedGenerator.Nodes.GetRandomElement();
            startNode.AlreadyUsedToSpawnRoom = true;
            List<FloorGeneratorNode> childNodes = SelectNeigbors(startNode, Random.Range(3, 6));
            List<FloorGeneratorNode> nodesToSpawn = new();
            nodesToSpawn.Add(startNode);
            nodesToSpawn.AddRange(childNodes);

            int expectedCountOfRooms = 16;

            while (nodesToSpawn.Count < expectedCountOfRooms - 1)
            {
                var selectedNode = childNodes.GetRandomElement();
                List<FloorGeneratorNode> newNodes = SelectNeigbors(selectedNode, Random.Range(1, 4));
                nodesToSpawn.AddRange(newNodes);
                childNodes.Remove(selectedNode);
                childNodes.AddRange(newNodes);
            }

            foreach (var node in nodesToSpawn)
            {
                var spawnedButton = SpawnRoomButton(node, distanceBetweenButtonsMultiplier);
                roomButtons.Add(spawnedButton);
            }

            Vector3 centerPosition = CalculateCenterPositionOfAllSpawnedButtons() - Vector2.zero;

            foreach (var spawnedButton in roomButtons)
            {
                spawnedButton.transform.localPosition -= centerPosition;
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
            print(centerY);
            return new Vector2 (centerX, centerY);
        }

        private RoomButton SpawnRoomButton(FloorGeneratorNode selectedNode, float distanceBetweenButtonsMultiplier)
        {
            selectedNode.AlreadyUsedToSpawnRoom = true;
            var theFirstRoom = Instantiate(roomButtonPrefab, map);
            theFirstRoom.transform.localPosition = new Vector3(selectedNode.Coordinates.x, selectedNode.Coordinates.y) * distanceBetweenButtonsMultiplier;
            return theFirstRoom;
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