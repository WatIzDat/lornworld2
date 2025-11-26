using MemoryPack;

[MemoryPackable]
[MemoryPackUnion(0, typeof(EmptyFeatureData))]
[MemoryPackUnion(1, typeof(SceneEntranceFeatureData))]
[MemoryPackUnion(2, typeof(OreFeatureData))]
public abstract partial class FeatureData
{
}
