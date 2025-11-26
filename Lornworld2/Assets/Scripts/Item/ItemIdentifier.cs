using MemoryPack;

[MemoryPackable]
public partial record ItemIdentifier : Identifier
{
    public ItemIdentifier(string id) : base(id) { }
}
