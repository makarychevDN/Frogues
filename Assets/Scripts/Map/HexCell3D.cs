using System;
using FroguesFramework;
using UnityEngine;

[ExecuteAlways]
public class HexCell3D : MonoBehaviour
{
    [SerializeField] private Vector3 _hashedPosition;
    [field : SerializeField] public Vector2Int GridPosition { get; set; }

    private void Update()
    {
        if (_hashedPosition == transform.position)
            return;
        
        _hashedPosition = transform.position;
        //transform.GetComponentInParent<HexMap3D>()?.SetCell(this);
    }

    private void OnDestroy()
    {
        //transform.GetComponentInParent<HexMap3D>()?.RemoveCell(this);
    }
}
