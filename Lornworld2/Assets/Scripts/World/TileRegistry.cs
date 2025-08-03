using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileRegistry : Registry<TileScriptableObject, TileIdentifier>
{
    public static TileRegistry Instance { get; private set; }
    protected override Type IdsType { get; } = typeof(TileIds);

    private readonly Dictionary<Tile, TileScriptableObject> placeholderTileToTileObjMap = new();
    private readonly Dictionary<Tile, Tile[]> placeholderTileToDisplayTilesMap = new();

    [SerializeField]
    private string tilesResourcesPath = "Tiles";

    protected override void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);

            Debug.LogError("Can't create another TileRegistry instance");
        }
        else
        {
            Instance = this;
        }

        base.Awake();

        foreach (TileScriptableObject tile in Entries)
        {
            placeholderTileToTileObjMap.Add(tile.placeholderTile, tile);

            string displayTilesDirectoryName = tile.tileName;
            Tile placeholderTile = tile.placeholderTile;

            Tile[] tiles = Resources.LoadAll<Tile>($"{tilesResourcesPath}/{displayTilesDirectoryName}");

            Array.Sort(tiles, (x, y) => string.Compare(x.name, y.name));

            placeholderTileToDisplayTilesMap.Add(placeholderTile, tiles);
        }
    }

    public TileScriptableObject GetScriptableObjectFromPlaceholderTile(Tile placeholderTile)
    {
        if (placeholderTile == null)
        {
            return null;
        }

        return placeholderTileToTileObjMap[placeholderTile];
    }

    public Tile[] GetDisplayTilesFromPlaceholderTile(Tile placeholderTile)
    {
        return placeholderTileToDisplayTilesMap[placeholderTile];
    }

    protected override TileIdentifier CreateId(string id)
    {
        return new TileIdentifier(id);
    }
}
