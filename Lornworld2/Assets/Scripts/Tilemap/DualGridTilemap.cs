using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DualGridTilemap : MonoBehaviour
{
    private static readonly Vector2Int[] neighbours = new Vector2Int[]
    {
        new(0, 0),
        new(1, 0),
        new(0, 1),
        new(1, 1)
    };

    private static Dictionary<Tuple<bool, bool, bool, bool>, Tile> neighbourStateToTile;

    [SerializeField]
    private Tilemap worldTilemap;

    [SerializeField]
    private Tilemap displayTilemap;

    // make this into dictionary
    [SerializeField]
    private Tile[] tiles = new Tile[16];

    private void Awake()
    {
        neighbourStateToTile = new()
        {
            {new (true, true, true, true), tiles[6]},
            {new (false, false, false, true), tiles[13]}, // OUTER_BOTTOM_RIGHT
            {new (false, false, true, false), tiles[0]}, // OUTER_BOTTOM_LEFT
            {new (false, true, false, false), tiles[8]}, // OUTER_TOP_RIGHT
            {new (true, false, false, false), tiles[15]}, // OUTER_TOP_LEFT
            {new (false, true, false, true), tiles[1]}, // EDGE_RIGHT
            {new (true, false, true, false), tiles[11]}, // EDGE_LEFT
            {new (false, false, true, true), tiles[3]}, // EDGE_BOTTOM
            {new (true, true, false, false), tiles[9]}, // EDGE_TOP
            {new (false, true, true, true), tiles[5]}, // INNER_BOTTOM_RIGHT
            {new (true, false, true, true), tiles[2]}, // INNER_BOTTOM_LEFT
            {new (true, true, false, true), tiles[10]}, // INNER_TOP_RIGHT
            {new (true, true, true, false), tiles[7]}, // INNER_TOP_LEFT
            {new (false, true, true, false), tiles[14]}, // DUAL_UP_RIGHT
            {new (true, false, false, true), tiles[4]}, // DUAL_DOWN_RIGHT
            {new (false, false, false, false), tiles[12]},
        };
    }

    private TileBase GetWorldTile(Vector2Int position)
    {
        // make this into cast
        return worldTilemap.GetTile(new Vector3Int(position.x, position.y, 0));
    }

    public void SetWorldTile(Vector2Int position, Tile tile)
    {
        worldTilemap.SetTile(new Vector3Int(position.x, position.y, 0), tile);
        SetDisplayTile((Vector3Int)position, tile);
    }

    private Tile GetDisplayTile(Vector2Int displayPosition, Tile tile)
    {
        bool topRight = GetWorldTile(displayPosition - neighbours[0]);
        bool topLeft = GetWorldTile(displayPosition - neighbours[1]);
        bool botRight = GetWorldTile(displayPosition - neighbours[2]);
        bool botLeft = GetWorldTile(displayPosition - neighbours[3]);

        Tuple<bool, bool, bool, bool> neighbourState = new(topLeft, topRight, botLeft, botRight);

        return neighbourStateToTile[neighbourState];
    }

    private void SetDisplayTile(Vector3Int position, Tile tile)
    {
        foreach (Vector2Int neighbourPos in neighbours)
        {
            Vector3Int offsetPos = position + new Vector3Int(neighbourPos.x, neighbourPos.y, 0);

            displayTilemap.SetTile(
                offsetPos,
                GetDisplayTile(new Vector2Int(offsetPos.x, offsetPos.y), tile));
        }
    }
}
