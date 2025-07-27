using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk : MonoBehaviour
{
    // array is bottom left to top right counting left to right
    private readonly ObservableCollection<TileScriptableObject> tiles = new();

    private DualGridTilemap tilemap;

    public ChunkPos chunkPos;

    public static event Action<ChunkPos, int, TileScriptableObject> ChunkChanged;

    //public Chunk(ChunkPos chunkPos)
    //{
    //    this.chunkPos = chunkPos;

    //    tiles.CollectionChanged += OnTilesChanged;
    //}

    public static Chunk Create(ChunkPos chunkPos, GameObject chunkPrefab, Transform chunkParent)
    {
        Chunk chunk = 
            Instantiate(
                chunkPrefab,
                new Vector3(chunkPos.pos.x, chunkPos.pos.y, 0) * ChunkManager.ChunkSize,
                Quaternion.identity,
                chunkParent)
            .GetComponent<Chunk>();

        chunk.chunkPos = chunkPos;

        chunk.tiles.CollectionChanged += chunk.OnTilesChanged;

        return chunk;
    }

    public static Chunk Pool(ChunkPos chunkPos, GameObject unusedChunk)
    {
        unusedChunk.transform.position = new Vector3(chunkPos.pos.x, chunkPos.pos.y, 0) * ChunkManager.ChunkSize;
        //unusedChunk.SetActive(true);
        unusedChunk.transform.GetChild(0).GetChild(1).GetComponent<TilemapRenderer>().enabled = true;

        Chunk chunk = unusedChunk.GetComponent<Chunk>();

        chunk.chunkPos = chunkPos;

        chunk.tilemap.ClearAllTiles();
        chunk.tiles.Clear();

        return chunk;
    }

    private void Awake()
    {
        tilemap = GetComponentInChildren<DualGridTilemap>();
    }

    private void OnTilesChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        //if (e.NewItems == null)
        //{
        //    return;
        //}

        //foreach (TileScriptableObject tile in e.NewItems)
        //{
        //    Vector2Int pos = new(
        //        e.NewStartingIndex % ChunkManager.ChunkSize,
        //        e.NewStartingIndex / ChunkManager.ChunkSize);

        //    // TODO: use SetTileBlock instead of SetTile for performance
        //    tilemap.SetTile(pos, tile);

        //    ChunkChanged?.Invoke(chunkPos, e.NewStartingIndex, tile);
        //}
    }

    public void PopulateWith(Func<ChunkPos, TileScriptableObject[]> generate)
    {
        TileScriptableObject[] generatedTiles = generate(chunkPos);

        tiles.AddRange(generatedTiles);

        BoundsInt bounds = new(0, 0, 0, ChunkManager.ChunkSize, ChunkManager.ChunkSize, 1);

        tilemap.SetTilesBlock(bounds, generatedTiles);
    }
}
