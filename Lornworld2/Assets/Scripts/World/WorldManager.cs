using UnityEngine;

public class WorldManager : MonoBehaviour
{
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

    private void Start()
    {
        Generate(worldGenerator);
    }

    public void Generate(IWorldGenerator generator)
    {
        chunkManager.Generate(generator);

        pathfindingGrid.Initialize();

        //foreach (TileScriptableObject[] tiles in tilesInChunks)
        //{
        //    loadedTiles.AddRange(tiles);
        //}
    }
}
