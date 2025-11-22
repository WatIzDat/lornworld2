using MemoryPack;
using UnityEngine;

public record ChunkData
{
    public TileScriptableObject[] tiles;

    public (FeatureScriptableObject feature, Vector2 pos, FeatureData data)[] features;

    public ChunkData(TileScriptableObject[] tiles, (FeatureScriptableObject feature, Vector2 pos, FeatureData data)[] features)
    {
        this.tiles = tiles;
        this.features = features;
    }
}

[MemoryPackable]
public partial record ChunkDataPersistence : IGameData
{
    public TileIdentifier[] tiles;

    public (FeatureIdentifier feature, Vector2 pos, FeatureData data)[] features;

    public ChunkDataPersistence(TileIdentifier[] tiles, (FeatureIdentifier feature, Vector2 pos, FeatureData data)[] features)
    {
        this.tiles = tiles;
        this.features = features;
    }
}
