using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Unity.VisualScripting;

public class Chunk
{
    // array is bottom left to top right counting left to right
    private readonly ObservableCollection<TileScriptableObject> tiles = new();

    public ChunkPos chunkPos;

    public static event Action<ChunkPos, int, TileScriptableObject> ChunkChanged;

    public Chunk(ChunkPos chunkPos)
    {
        this.chunkPos = chunkPos;

        tiles.CollectionChanged += OnTilesChanged;
    }

    private void OnTilesChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        foreach (TileScriptableObject tile in e.NewItems)
        {
            ChunkChanged?.Invoke(chunkPos, e.NewStartingIndex, tile);
        }
    }

    public void PopulateWith(Func<ChunkPos, TileScriptableObject[]> generate)
    {
        TileScriptableObject[] generatedTiles = generate(chunkPos);

        tiles.AddRange(generatedTiles);
    }
}
