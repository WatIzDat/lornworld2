using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Unity.VisualScripting;
using UnityEngine;

public class Chunk
{
    private readonly ObservableCollection<TileScriptableObject> tiles = new();

    public ChunkPos chunkPos;

    public event Action<int, TileScriptableObject> ChunkChanged;

    public Chunk(ChunkPos chunkPos)
    {
        this.chunkPos = chunkPos;

        tiles.CollectionChanged += OnTilesChanged;
    }

    private void OnTilesChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        foreach (TileScriptableObject tile in e.NewItems)
        {
            ChunkChanged?.Invoke(e.NewStartingIndex, tile);
        }
    }

    public void PopulateWith(Func<ChunkPos, TileScriptableObject[]> generate)
    {
        TileScriptableObject[] generatedTiles = generate(chunkPos);

        tiles.AddRange(generatedTiles);
    }
}
