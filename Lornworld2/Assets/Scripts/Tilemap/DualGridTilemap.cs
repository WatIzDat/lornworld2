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

    //[SerializeField]
    //private string tilesResourcesPath = "Tiles";

    //private readonly Dictionary<Tile, Tile[]> placeholderTileToDisplayTiles = new();

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
        //foreach (TileScriptableObject tileObj in TileRegistry.Instance.Tiles)
        //{
        //    string tileDirectoryName = tileObj.tileName;
        //    Tile placeholderTile = tileObj.placeholderTile;

        //    Tile[] tiles = Resources.LoadAll<Tile>($"{tilesResourcesPath}/{tileDirectoryName}");

        //    Array.Sort(tiles, (x, y) => string.Compare(x.name, y.name));

        //    placeholderTileToDisplayTiles.Add(placeholderTile, tiles);
        //}

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

    public void SetTilesBlock(BoundsInt bounds, TileScriptableObject[] tileObjs)
    {
        TileBase[] tiles = new TileBase[bounds.size.x * bounds.size.y];

        for (int i = 0; i < tileObjs.Length; i++)
        {
            tiles[i] = tileObjs[i].placeholderTile;
        }

        worldTilemap.SetTilesBlock(bounds, tiles);
        SetDisplayTilesBlock(bounds, tileObjs);
        //displayTilemap.SetTile(new Vector3Int(0, 0, 0), TileRegistry.Instance.GetTile(TileIds.Grass).placeholderTile);
    }

    public void DeleteTile(Vector2Int position)
    {
        TileScriptableObject tile = GetWorldTile(position);
        worldTilemap.SetTile((Vector3Int)position, null);
        SetDisplayTile(position, tile);
    }

    public void ClearAllTiles()
    {
        worldTilemap.ClearAllTiles();
        displayTilemap.ClearAllTiles();
    }

    private (Tile tile, TileState tileState) GetDisplayTile(Vector2Int displayPosition, TileScriptableObject tile)
    {
        bool topRight = GetWorldTile(displayPosition - neighbours[0]) == tile;
        bool topLeft = GetWorldTile(displayPosition - neighbours[1]) == tile;
        bool botRight = GetWorldTile(displayPosition - neighbours[2]) == tile;
        bool botLeft = GetWorldTile(displayPosition - neighbours[3]) == tile;

        Tuple<bool, bool, bool, bool> neighbourState = new(topLeft, topRight, botLeft, botRight);

        Tile[] displayTiles = TileRegistry.Instance.GetDisplayTilesFromPlaceholderTile(tile.placeholderTile);
        TileState tileState = neighbourStateToTileState[neighbourState];
        int displayTileIndex = (int)tileState;

        return (displayTiles[displayTileIndex], tileState);
    }

    private void SetDisplayTile(Vector2Int position, TileScriptableObject tile)
    {
        foreach (Vector2Int neighbourPos in neighbours)
        {
            int z = tile.order;

            Vector3Int offsetPos = (Vector3Int)position + new Vector3Int(neighbourPos.x, neighbourPos.y, z);

            displayTilemap.SetTile(
                offsetPos,
                GetDisplayTile((Vector2Int)offsetPos, tile).tile);
        }
    }

    private void SetDisplayTilesBlock(BoundsInt bounds, TileScriptableObject[] tiles)
    {
        // make the z bound change also
        BoundsInt newBounds = new(0, 0, 0, bounds.size.x + 1, bounds.size.y + 1, 1);

        Tile[] pendingDisplayTileChanges = new Tile[newBounds.size.x * newBounds.size.y];

        int j = 0;

        for (int i = 0; i < tiles.Length; i++)
        {
            int z = tiles[i].order;
            //Debug.Log(position);

            if ((j - (j / newBounds.size.x)) % bounds.size.x == 0 && j != 0)
            {
                j++;
            }

            Vector3Int position = new(j % newBounds.size.x, j / newBounds.size.y, z);
            //Debug.Log(j + " " + i);

            foreach (Vector2Int neighbourPos in neighbours)
            {
                Vector3Int offsetPos = position + new Vector3Int(neighbourPos.x, neighbourPos.y, z);


                int index = (j + neighbourPos.x) + (neighbourPos.y * newBounds.size.x);

                (Tile, TileState) test = GetDisplayTile((Vector2Int)offsetPos, tiles[i]);

                pendingDisplayTileChanges[index] = test.Item1;

                //Debug.Log(index + " " + j + " " + test.Item2 + " " + position);
            }

            j++;
        }

        displayTilemap.SetTilesBlock(newBounds, pendingDisplayTileChanges);
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
