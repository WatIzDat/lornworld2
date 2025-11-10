using MemoryPack;

[MemoryPackable]
public partial record TileIdentifier : Identifier
{
    //public string Id { get; }

    public TileIdentifier(string id) : base(id) { }
}
