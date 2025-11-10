using MemoryPack;

[MemoryPackable]
[MemoryPackUnion(0, typeof(TileIdentifier))]
[MemoryPackUnion(1, typeof(FeatureIdentifier))]
public abstract partial record Identifier
{
    public string Id { get; }

    public Identifier(string id)
    {
        Id = id;
    }
}
