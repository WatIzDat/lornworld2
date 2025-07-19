using System;
using System.Collections.Generic;
using System.Linq;
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

    private static Dictionary<Tuple<bool, bool, bool, bool>, TileState> neighbourStateToTileState;

    [SerializeField]
    private Tilemap worldTilemap;

    [SerializeField]
    private Tilemap displayTilemap;

    [SerializeField]
    private string tilesResourcesPath = "Tiles";

    [SerializeField]
    private TileNameToPlaceholderTileMapping[] tileDirectoryNameToPlaceholderTile;

    // make this into dictionary
    //[SerializeField]
    //private Tile[] tiles = new Tile[16];

    // make this dictionary into a separate class
    private readonly System.Collections.Generic.SortedDictionary<Tile, Tile[]> placeholderTileToTiles = new();

    private void Awake()
    {
        foreach (TileNameToPlaceholderTileMapping mapping in tileDirectoryNameToPlaceholderTile)
        {
            string tileDirectoryName = mapping.name;
            Tile placeholderTile = mapping.tile;

            Tile[] tiles = Resources.LoadAll<Tile>($"{tilesResourcesPath}/{tileDirectoryName}");

            Array.Sort(tiles, (x, y) => string.Compare(x.name, y.name));

            placeholderTileToTiles.Add(placeholderTile, tiles);
        }

        neighbourStateToTileState = new()
        {
            {new (true, true, true, true), TileState.Full},
            {new (false, false, false, true), TileState.OuterBottomRight}, // OUTER_BOTTOM_RIGHT
            {new (false, false, true, false), TileState.OuterBottomLeft}, // OUTER_BOTTOM_LEFT
            {new (false, true, false, false), TileState.OuterTopRight}, // OUTER_TOP_RIGHT
            {new (true, false, false, false), TileState.OuterTopLeft}, // OUTER_TOP_LEFT
            {new (false, true, false, true), TileState.EdgeRight}, // EDGE_RIGHT
            {new (true, false, true, false), TileState.EdgeLeft}, // EDGE_LEFT
            {new (false, false, true, true), TileState.EdgeBottom}, // EDGE_BOTTOM
            {new (true, true, false, false), TileState.EdgeTop}, // EDGE_TOP
            {new (false, true, true, true), TileState.InnerBottomRight}, // INNER_BOTTOM_RIGHT
            {new (true, false, true, true), TileState.InnerBottomLeft}, // INNER_BOTTOM_LEFT
            {new (true, true, false, true), TileState.InnerTopRight}, // INNER_TOP_RIGHT
            {new (true, true, true, false), TileState.InnerTopLeft}, // INNER_TOP_LEFT
            {new (false, true, true, false), TileState.DualUpRight}, // DUAL_UP_RIGHT
            {new (true, false, false, true), TileState.DualDownRight}, // DUAL_DOWN_RIGHT
            {new (false, false, false, false), TileState.Empty},
        };
    }

    private void Start()
    {
        RefreshDisplayTilemap();
    }

    private Tile GetWorldTile(Vector2Int position)
    {
        // make this into cast
        return worldTilemap.GetTile<Tile>(new Vector3Int(position.x, position.y, 0));
    }

    public void SetTile(Vector2Int position, Tile tile)
    {
        worldTilemap.SetTile(new Vector3Int(position.x, position.y, 0), tile);
        SetDisplayTile((Vector3Int)position, tile);
    }

    private Tile GetDisplayTile(Vector2Int displayPosition, Tile tile)
    {
        bool topRight = GetWorldTile(displayPosition - neighbours[0]) == tile;
        bool topLeft = GetWorldTile(displayPosition - neighbours[1]) == tile;
        bool botRight = GetWorldTile(displayPosition - neighbours[2]) == tile;
        bool botLeft = GetWorldTile(displayPosition - neighbours[3]) == tile;

        Tuple<bool, bool, bool, bool> neighbourState = new(topLeft, topRight, botLeft, botRight);

        Tile[] displayTiles = placeholderTileToTiles[tile];
        int displayTileIndex = (int)neighbourStateToTileState[neighbourState];

        return displayTiles[displayTileIndex];
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

    private void RefreshDisplayTilemap()
    {
        for (int i = -50; i < 50; i++)
        {
            for (int j = -50; j < 50; j++)
            {
                Vector3Int pos = new(i, j, 0);

                Tile tile = GetWorldTile(new Vector2Int(i, j));

                if (tile == null)
                {
                    continue;
                }

                SetDisplayTile(pos, tile);
            }
        }
    }
}
