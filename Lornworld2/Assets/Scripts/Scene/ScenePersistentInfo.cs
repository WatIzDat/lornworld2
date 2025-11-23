public static class ScenePersistentInfo
{
    public static string PrevSceneId { get; private set; }

    private static string sceneId = "surface";
    public static string SceneId
    {
        get => sceneId;
        set
        {
            PrevSceneId = sceneId;
            sceneId = value;
        }
    }
}
