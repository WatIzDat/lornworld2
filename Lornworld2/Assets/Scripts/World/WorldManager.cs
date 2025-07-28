using Unity.VisualScripting;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField]
    private ChunkManager chunkManager;

    private TileScriptableObject[] loadedTiles;

    private void Start()
    {
        Generate(new DebugWorldGenerator());
    }

    public void Generate(IWorldGenerator generator)
    {
        TileScriptableObject[][] tilesInChunks = chunkManager.Generate(generator);

        //foreach (TileScriptableObject[] tiles in tilesInChunks)
        //{
        //    loadedTiles.AddRange(tiles);
        //}
    }
}
