using MemoryPack;

[MemoryPackable]
public partial record FeatureIdentifier : Identifier
{
    public FeatureIdentifier(string id) : base(id) { }
}
