using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileRegistry : MonoBehaviour
{
    public static TileRegistry Instance { get; private set; }

    public TileScriptableObject[] Tiles { get; private set; }

    private Dictionary<Tile, TileScriptableObject> placeholderTileToTileObjMap = new();

    [SerializeField]
    private string tileScriptableObjectsPath;

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

        foreach (TileScriptableObject tile in Tiles)
        {
            placeholderTileToTileObjMap.Add(tile.placeholderTile, tile);

            Debug.Log(tile.tileName);
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
}
