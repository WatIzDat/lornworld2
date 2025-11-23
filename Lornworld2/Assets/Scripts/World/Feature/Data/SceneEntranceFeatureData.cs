using MemoryPack;

[MemoryPackable]
public partial class SceneEntranceFeatureData : FeatureData
{
    public int sceneBuildIndex;
    public string sceneId;

    public SceneEntranceFeatureData(int sceneBuildIndex, string sceneId)
    {
        this.sceneBuildIndex = sceneBuildIndex;
        this.sceneId = sceneId;
    }
}
