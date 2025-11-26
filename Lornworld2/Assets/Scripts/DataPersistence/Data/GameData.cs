using MemoryPack;

[MemoryPackable]
public partial class GameData : IGameData
{
    public string sceneId;
    public int sceneBuildIndex;

    public GameData(string sceneId, int sceneBuildIndex)
    {
        this.sceneId = sceneId;
        this.sceneBuildIndex = sceneBuildIndex;
    }
}
