public static class ScenePersistentInfo
{
    public static bool IsInitialScene { get; private set; } = true;

    public static string PrevSceneId { get; private set; }

    private static string sceneId = "surface";
    public static string SceneId
    {
        get => sceneId;
        set
        {
            PrevSceneId = sceneId;
            sceneId = value;

            IsInitialScene = false;
        }
    }

    public static void InitializeSceneId(string sceneId)
    {
        ScenePersistentInfo.sceneId = sceneId;

        IsInitialScene = false;
    }
}
