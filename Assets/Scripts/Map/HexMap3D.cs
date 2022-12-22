using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class HexMap3D : MonoBehaviour
    {
        [SerializeField] private List<HexCell3D> _hexCell3Ds;
        public Dictionary<MapLayer, Cell[,]> layers;
        private Cell[,] layer;
        private float _lowestXPosition, _lowestZPosition;
        private float _biggestXPosition, _biggestZPosition;
        private float _xStep = 0.8675f;
        private float _zStep = 1.5f;

        public void SetCell(HexCell3D hexCell3D)
        {
            _hexCell3Ds.RemoveAll(cell => cell == null);
            
            if(!_hexCell3Ds.Contains(hexCell3D))
                _hexCell3Ds.Add(hexCell3D);
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

            int sizeX = Convert.ToInt32((_biggestXPosition - _lowestXPosition) / _xStep);
            int sizeZ = Convert.ToInt32((_biggestZPosition - _lowestZPosition) / _zStep);

            layer = new Cell[sizeX, sizeZ];
        }
    }
}
