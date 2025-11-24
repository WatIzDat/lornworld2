using MemoryPack;

[MemoryPackable]
public partial class WorldData : IGameData
{
    public int worldSeed;

    public WorldData(int worldSeed)
    {
        this.worldSeed = worldSeed;
    }
}
