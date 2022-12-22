using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class HexMap3D : MonoBehaviour
    {
        [SerializeField] private List<HexCell3D> _hexCell3Ds = new List<HexCell3D>();

        public void SetCell(HexCell3D hexCell3D)
        {
            _hexCell3Ds.RemoveAll(cell => cell == null);
            
            if(!_hexCell3Ds.Contains(hexCell3D))
                _hexCell3Ds.Add(hexCell3D);
        }
    }
}
