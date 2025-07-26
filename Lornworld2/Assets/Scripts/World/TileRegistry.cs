using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileRegistry : MonoBehaviour
{
    public static TileRegistry Instance { get; private set; }

    public TileScriptableObject[] Tiles { get; private set; }

    public int MinOrder { get; private set; }
    public int MaxOrder { get; private set; }

    private readonly Dictionary<Tile, TileScriptableObject> placeholderTileToTileObjMap = new();
    private readonly Dictionary<TileIdentifier, TileScriptableObject> tileIdToTileObjMap = new();
    private readonly Dictionary<Tile, Tile[]> placeholderTileToDisplayTilesMap = new();

    [SerializeField]
    private string tileScriptableObjectsPath;

    [SerializeField]
    private string tilesResourcesPath = "Tiles";

    private void Awake()
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

        Tiles = Resources.LoadAll<TileScriptableObject>(tileScriptableObjectsPath);

        Assembly assembly = typeof(TileIds).Assembly;

        Type tileIdsType = assembly.GetType(nameof(TileIds));

        MinOrder = int.MaxValue;
        MaxOrder = int.MinValue;

        foreach (TileScriptableObject tile in Tiles)
        {
            placeholderTileToTileObjMap.Add(tile.placeholderTile, tile);

            TileIdentifier id = new(tile.tileName);

            tileIdsType.GetProperty(tile.tileName).SetValueOptimized(null, id);

            tileIdToTileObjMap.Add(id, tile);

            if (tile.order < MinOrder)
            {
                MinOrder = tile.order;
            }
            
            if (tile.order > MaxOrder)
            {
                MaxOrder = tile.order;
            }

            string displayTilesDirectoryName = tile.tileName;
            Tile placeholderTile = tile.placeholderTile;

            Tile[] tiles = Resources.LoadAll<Tile>($"{tilesResourcesPath}/{displayTilesDirectoryName}");

            Array.Sort(tiles, (x, y) => string.Compare(x.name, y.name));

            placeholderTileToDisplayTilesMap.Add(placeholderTile, tiles);

            Debug.Log(tile.tileName);
        }
    }

    public TileScriptableObject GetTile(TileIdentifier id)
    {
        return tileIdToTileObjMap[id];
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
}
