using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Unity.VisualScripting;
using UnityEngine;

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

    private void Awake()
    {
        tilemap = GetComponentInChildren<DualGridTilemap>();
    }

    private void OnTilesChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        foreach (TileScriptableObject tile in e.NewItems)
        {
            Vector2Int pos = new(
                e.NewStartingIndex % ChunkManager.ChunkSize,
                e.NewStartingIndex / ChunkManager.ChunkSize);

            tilemap.SetTile(pos, tile);

            ChunkChanged?.Invoke(chunkPos, e.NewStartingIndex, tile);
        }
    }

    public void PopulateWith(Func<ChunkPos, TileScriptableObject[]> generate)
    {
        TileScriptableObject[] generatedTiles = generate(chunkPos);

        tiles.AddRange(generatedTiles);
    }
}
