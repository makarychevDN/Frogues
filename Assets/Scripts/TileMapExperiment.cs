using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapExperiment : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile tile;


    void Start()
    {
        tilemap.SetTile(Vector3Int.zero, tile);

        BoundsInt bounds = tilemap.cellBounds;

        Vector3Int cellPosition = tilemap.layoutGrid.WorldToCell(transform.position);
        transform.position = tilemap.layoutGrid.CellToWorld(cellPosition);

        
        for (int i = 0; i < bounds.size.x; i++)
        {
            for (int j = 0; j < bounds.size.y; j++)
            {
                if(tilemap.GetTile(new Vector3Int(i, j, 0)) != null)
                    print(tilemap.CellToWorld(new Vector3Int(i, j, 0)));
            }
        }


    }
}
