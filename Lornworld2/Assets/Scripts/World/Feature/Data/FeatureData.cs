using MemoryPack;

[MemoryPackable]
[MemoryPackUnion(0, typeof(EmptyFeatureData))]
[MemoryPackUnion(1, typeof(SceneEntranceFeatureData))]
public abstract partial class FeatureData
{
}
