using FMODUnity;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour, IDataPersistence<WorldData>
{
    public int worldSeed;

    public bool hasAmbience;
    public EventReference ambienceAudio;

    public MusicArea musicArea;

    //[SerializeField]
    private ChunkManager chunkManager;

    [SerializeField]
    private WorldGeneratorScriptableObject worldGenerator;

    //[SerializeField]
    private PathfindingGrid pathfindingGrid;

    private void Awake()
    {
        chunkManager = FindFirstObjectByType<ChunkManager>();
        pathfindingGrid = FindFirstObjectByType<PathfindingGrid>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        ChunkManager.InitialChunksGenerated += OnInitialChunksGenerated;

        DataPersistenceManager.SaveTriggered += SaveData;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        ChunkManager.InitialChunksGenerated -= OnInitialChunksGenerated;

        DataPersistenceManager.SaveTriggered -= SaveData;
    }

    //private void Start()
    //{
    //    Generate(worldGenerator);
    //}

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (ScenePersistentInfo.IsInitialScene)
        {
            DataPersistenceManager.Instance.LoadObject<GameData>(
                data =>
                {
                    //ScenePersistentInfo.SceneId = data.sceneId;
                    ScenePersistentInfo.InitializeSceneId(data.sceneId);

                    SceneManager.LoadScene(data.sceneBuildIndex);
                },
                () => { },
                "game");
        }

        DataPersistenceManager.Instance.LoadObject<WorldData>(
            data => LoadData(data),
            () =>
            {
                worldSeed = Random.Range(int.MinValue, int.MaxValue);

                Generate(worldGenerator);
            },
            Path.Combine(ScenePersistentInfo.SceneId, "world"));
    }

    private void OnInitialChunksGenerated()
    {
        pathfindingGrid.Initialize();

        if (!chunkManager.IsShiftingChunks)
        {
            pathfindingGrid.CreateGrid();
        }
    }

    public void Generate(IWorldGenerator generator)
    {
        Debug.Log(worldSeed);
        chunkManager.Generate(generator.GetGenerator(worldSeed));

        //pathfindingGrid.Initialize();

        //foreach (TileScriptableObject[] tiles in tilesInChunks)
        //{
        //    loadedTiles.AddRange(tiles);
        //}
    }

    public bool LoadData(WorldData data)
    {
        Debug.Log("Loaded Seed: " + data.worldSeed);
        worldSeed = data.worldSeed;

        Generate(worldGenerator);

        return true;
    }

    public void SaveData(System.Action<IGameData, string> saveCallback, bool gameExit)
    {
        Debug.Log("Saved Seed: " + worldSeed);
        saveCallback(new WorldData(worldSeed), Path.Combine(ScenePersistentInfo.SceneId, "world"));

        if (gameExit)
        {
            saveCallback(new GameData(ScenePersistentInfo.SceneId, SceneManager.GetActiveScene().buildIndex), "game");
        }
    }
}
