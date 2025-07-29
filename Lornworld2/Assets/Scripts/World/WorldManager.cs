using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField]
    private ChunkManager chunkManager;

    private void Start()
    {
        Generate(new BasicWorldGenerator());
    }

    public void Generate(IWorldGenerator generator)
    {
        chunkManager.Generate(generator);

        //foreach (TileScriptableObject[] tiles in tilesInChunks)
        //{
        //    loadedTiles.AddRange(tiles);
        //}
    }
}
