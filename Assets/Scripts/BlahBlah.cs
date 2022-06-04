using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlahBlah : MonoBehaviour
{
    [SerializeField] private Tilemap grid;
    [SerializeField] private Tile tile;
    
    void Start()
    {
        grid.SetTile(Vector3Int.zero, tile);
        grid.SetTile(Vector3Int.up, tile);
        grid.SetTile(Vector3Int.right, tile);
        grid.SetTile(new Vector3Int(8, 8, 0), tile);
    }
}
