using MemoryPack;

[MemoryPackable]
public partial class OreFeatureData : FeatureData
{
    public OreType oreType; 

    public OreFeatureData(OreType oreType)
    {
        this.oreType = oreType;
    }
}
