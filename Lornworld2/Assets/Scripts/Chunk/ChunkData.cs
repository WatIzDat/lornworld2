using UnityEngine;

public record ChunkData
{
    public TileScriptableObject[] tiles;

    public (FeatureScriptableObject feature, Vector2 pos)[] features;

    public ChunkData(TileScriptableObject[] tiles, (FeatureScriptableObject feature, Vector2 pos)[] features)
    {
        this.tiles = tiles;
        this.features = features;
    }
}
