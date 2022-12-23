using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class HexMap3D : MonoBehaviour
    {
        [SerializeField] private List<Cell> _hexCell3Ds;
        public Dictionary<MapLayer, Cell[,]> layers;
        private Cell[,] layer;
        private float _lowestXPosition, _lowestZPosition;
        private float _biggestXPosition, _biggestZPosition;
        private float _xStep = 0.8675f;
        private float _zStep = 1.5f;

        public void SetCell(Cell hexCell3D)
        {
            _hexCell3Ds.RemoveAll(cell => cell == null);
            
            if(!_hexCell3Ds.Contains(hexCell3D))
                _hexCell3Ds.Add(hexCell3D);
        }
        
        public void RemoveCell(Cell hexCell3D)
        {
            _hexCell3Ds.RemoveAll(cell => cell == null);
            
            if(_hexCell3Ds.Contains(hexCell3D))
                _hexCell3Ds.Remove(hexCell3D);
        }

        private void Start()
        {
            Init();   
        }

        private void Init()
        {
            _lowestXPosition = _hexCell3Ds.Select(cell => cell.transform.localPosition.x).Min();
            _lowestZPosition = _hexCell3Ds.Select(cell => cell.transform.localPosition.z).Min();
            _biggestXPosition = _hexCell3Ds.Select(cell => cell.transform.localPosition.x).Max();
            _biggestZPosition = _hexCell3Ds.Select(cell => cell.transform.localPosition.z).Max();

            // +2 for cells with walls
            int sizeX = Convert.ToInt32((_biggestXPosition - _lowestXPosition) / _xStep) + 2;
            int sizeZ = Convert.ToInt32((_biggestZPosition - _lowestZPosition) / _zStep) + 2;

            layer = new Cell[sizeX, sizeZ];

            foreach (var hex in _hexCell3Ds)
            {
                hex.coordinates = GetGridPosition(hex);
            }

        }

        private Vector2Int GetGridPosition(Cell cell) => new Vector2Int(
            Convert.ToInt32((cell.transform.localPosition.x - _lowestXPosition - OddXModificator(cell)) / (_xStep * 2) + 1),
            Convert.ToInt32((cell.transform.localPosition.z - _lowestZPosition) / _zStep) + 1);

        private float OddXModificator(Cell cell)
        {
            return cell.transform.localPosition.z / _zStep % 2 * _xStep;
        }
    }
}
