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

    private static Dictionary<Tuple<bool, bool, bool, bool>, TileState> neighbourStateToTileState;

    [SerializeField]
    private Tilemap worldTilemap;

    [SerializeField]
    private Tilemap displayTilemap;

    [SerializeField]
    private string tilesResourcesPath = "Tiles";

    private readonly Dictionary<Tile, Tile[]> placeholderTileToDisplayTiles = new();

    private void Awake()
    {
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
        foreach (TileScriptableObject tileObj in TileRegistry.Instance.Tiles)
        {
            string tileDirectoryName = tileObj.tileName;
            Tile placeholderTile = tileObj.placeholderTile;

            Tile[] tiles = Resources.LoadAll<Tile>($"{tilesResourcesPath}/{tileDirectoryName}");

            Array.Sort(tiles, (x, y) => string.Compare(x.name, y.name));

            placeholderTileToDisplayTiles.Add(placeholderTile, tiles);
        }

        worldTilemap.GetComponent<TilemapRenderer>().enabled = false;

        RefreshDisplayTilemap();
    }

    private TileScriptableObject GetWorldTile(Vector2Int position)
    {
        Tile placeholderTile = worldTilemap.GetTile<Tile>((Vector3Int)position);

        return TileRegistry.Instance.GetScriptableObjectFromPlaceholderTile(placeholderTile);
    }

    public void SetTile(Vector2Int position, TileScriptableObject tileObj)
    {
        worldTilemap.SetTile((Vector3Int)position, tileObj.placeholderTile);
        SetDisplayTile(position, tileObj);
    }

    public void DeleteTile(Vector2Int position)
    {
        worldTilemap.SetTile((Vector3Int)position, null);
        DeleteDisplayTile(position);
    }

    private Tile GetDisplayTile(Vector2Int displayPosition, TileScriptableObject tile)
    {
        bool topRight = GetWorldTile(displayPosition - neighbours[0]) == tile;
        bool topLeft = GetWorldTile(displayPosition - neighbours[1]) == tile;
        bool botRight = GetWorldTile(displayPosition - neighbours[2]) == tile;
        bool botLeft = GetWorldTile(displayPosition - neighbours[3]) == tile;

        Tuple<bool, bool, bool, bool> neighbourState = new(topLeft, topRight, botLeft, botRight);

        Tile[] displayTiles = placeholderTileToDisplayTiles[tile.placeholderTile];
        int displayTileIndex = (int)neighbourStateToTileState[neighbourState];

        return displayTiles[displayTileIndex];
    }

    private void SetDisplayTile(Vector2Int position, TileScriptableObject tile)
    {
        foreach (Vector2Int neighbourPos in neighbours)
        {
            int z = tile.order;

            Vector3Int offsetPos = (Vector3Int)position + new Vector3Int(neighbourPos.x, neighbourPos.y, z);

            displayTilemap.SetTile(
                offsetPos,
                GetDisplayTile((Vector2Int)offsetPos, tile));
        }
    }

    private void DeleteDisplayTile(Vector2Int position)
    {
        foreach (Vector2Int neighbourPos in neighbours)
        {
            for (int z = TileRegistry.Instance.MinOrder; z < TileRegistry.Instance.MaxOrder; z++)
            {
                Vector3Int offsetPos = (Vector3Int)position + new Vector3Int(neighbourPos.x, neighbourPos.y, z);

                displayTilemap.SetTile(offsetPos, null);
            }
        }
    }

    private void RefreshDisplayTilemap()
    {
        for (int i = -50; i < 50; i++)
        {
            for (int j = -50; j < 50; j++)
            {
                Vector2Int pos = new(i, j);

                TileScriptableObject tile = GetWorldTile(pos);

                if (tile == null)
                {
                    continue;
                }

                SetDisplayTile(pos, tile);
            }
        }
    }
}
