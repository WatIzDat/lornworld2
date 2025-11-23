using MemoryPack;

[MemoryPackable]
public partial class SceneEntranceFeatureData : FeatureData
{
    public int sceneBuildIndex;

    public SceneEntranceFeatureData(int sceneBuildIndex)
    {
        this.sceneBuildIndex = sceneBuildIndex;
    }
}
