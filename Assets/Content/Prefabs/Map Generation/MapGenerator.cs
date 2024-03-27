using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace FroguesFramework
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private Texture2D voronoiGraphTexture;
        [SerializeField] private LineRenderer trailPrefab;
        [SerializeField] private GameObject nodePrefab;
        private Color _colorOfNodeOnTexture = Color.red;
        private float _distanceMultiplier = 1f;
        private List<MapGeneratorNode> _nodes = new();
        private List<Color> _backgroundColors = new() { Color.white, Color.black };

        private void Start()
        {
            for (int i = 0; i < voronoiGraphTexture.width; i++)
            {
                for (int j = 0; j < voronoiGraphTexture.height; j++)
                {
                    if (voronoiGraphTexture.GetPixel(i, j) == _colorOfNodeOnTexture)
                    {
                        MapGeneratorNode node = new MapGeneratorNode(new Vector2Int(i, j));
                        _nodes.Add(node);
                        node.FindKeysNearbyOnTexture(voronoiGraphTexture, _backgroundColors);
                    }
                }
            }

            foreach (MapGeneratorNode node in _nodes)
            {
                var nodeGameObject = Instantiate(nodePrefab);
                nodeGameObject.transform.position = new Vector3(node.Coordinates.x, 0, node.Coordinates.y);
                nodeGameObject.transform.parent = transform;

                Dictionary<Color, MapGeneratorNode> copyOfCollection = new Dictionary<Color, MapGeneratorNode>(node.Neighbors);
                foreach (Color key in copyOfCollection.Keys)
                {
                    if (node.Neighbors[key] != null)
                        continue;

                    var pairOfNode = _nodes.FirstOrDefault(otherNode => otherNode != node && otherNode.Neighbors.ContainsKey(key));
                    if (pairOfNode == null)
                    {
                        nodeGameObject.transform.localScale = new Vector3(10, 10, 10);
                        continue;
                    }

                    node.Neighbors[key] = pairOfNode;
                    pairOfNode.Neighbors[key] = node;

                    var line = Instantiate(trailPrefab, transform);
                    line.SetPosition(0, new Vector3(node.Coordinates.x, 0, node.Coordinates.y));
                    line.SetPosition(1, new Vector3(pairOfNode.Coordinates.x, 0, pairOfNode.Coordinates.y));
                }
            }
        }
    }

    public class MapGeneratorNode
    {
        private Vector2Int _coordinates;
        private Dictionary<Color, MapGeneratorNode> _neighbors;

        public MapGeneratorNode(Vector2Int coordinates)
        {
            _coordinates = coordinates;
            _neighbors = new Dictionary<Color, MapGeneratorNode>();
        }

        public Dictionary<Color, MapGeneratorNode> Neighbors => _neighbors;
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
    }
}