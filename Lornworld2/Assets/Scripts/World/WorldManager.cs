using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField]
    private ChunkManager chunkManager;

    [SerializeField]
    private PathfindingGrid pathfindingGrid;

    private void Start()
    {
        Generate(new BasicWorldGenerator());
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
