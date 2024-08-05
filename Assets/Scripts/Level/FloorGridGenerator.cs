using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class FloorGridGenerator : MonoBehaviour
    {
        [SerializeField] private Texture2D voronoiGraphTexture;
        [SerializeField] private LineRenderer trailPrefab;
        [SerializeField] private GameObject nodePrefab;

        private Color _colorOfNodeOnTexture = Color.red;
        private List<Color> _backgroundColors = new() { Color.white, Color.black };
        private List<FloorGeneratorNode> _nodes = new();

        public List<FloorGeneratorNode> Nodes { get {  return _nodes; } }

        public void GenerateFloorGrid()
        {
            GenerateNodes();
            FindNeighbors();
        }

        private void GenerateNodes()
        {
            for (int i = 0; i < voronoiGraphTexture.width; i++)
            {
                for (int j = 0; j < voronoiGraphTexture.height; j++)
                {
                    if (voronoiGraphTexture.GetPixel(i, j) == _colorOfNodeOnTexture)
                    {
                        FloorGeneratorNode node = new FloorGeneratorNode(new Vector2Int(i, j));
                        _nodes.Add(node);
                        node.FindKeysNearbyOnTexture(voronoiGraphTexture, _backgroundColors);
                    }
                }
            }
        }

        private void FindNeighbors()
        {
            foreach (FloorGeneratorNode node in _nodes)
            {
                Dictionary<Color, FloorGeneratorNode> copyOfCollection = new(node.Neighbors);

                foreach (Color key in copyOfCollection.Keys)
                {
                    if (node.Neighbors[key] != null)
                        continue;

                    var pairOfNode = _nodes.FirstOrDefault(otherNode => otherNode != node && otherNode.Neighbors.ContainsKey(key));
                    node.Neighbors[key] = pairOfNode;
                    pairOfNode.Neighbors[key] = node;
                }
            }
        }
    }

    public class FloorGeneratorNode
    {
        private Vector2Int _coordinates;
        private Dictionary<Color, FloorGeneratorNode> _neighbors;
        private bool alreadyUsedToSpawnRoom;

        public bool AlreadyUsedToSpawnRoom { get => alreadyUsedToSpawnRoom; set => alreadyUsedToSpawnRoom = value; }

        public FloorGeneratorNode(Vector2Int coordinates)
        {
            _coordinates = coordinates;
            _neighbors = new Dictionary<Color, FloorGeneratorNode>();
        }

        public Dictionary<Color, FloorGeneratorNode> Neighbors => _neighbors;
        public Vector2Int Coordinates => _coordinates;

        public void FindKeysNearbyOnTexture(Texture2D texture, List<Color> colorsToIgnore)
        {
            for(int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0)
                        continue;

                    Color currentColor = texture.GetPixel(i + _coordinates.x, j + _coordinates.y);
                    if (colorsToIgnore.Contains(currentColor) || _neighbors.Keys.Contains(currentColor))
                        continue;

                    _neighbors.Add(currentColor, null);
                }
            }
        }

        public float DistanceToOtherNode(FloorGeneratorNode otherNode)
        {
            return Vector2.Distance(otherNode._coordinates, _coordinates);
        }
    }
}