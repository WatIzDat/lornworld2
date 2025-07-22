using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldManager : MonoBehaviour
{
    [SerializeField]
    private ChunkManager chunkManager;

    public Tile[] tileRegistry;

    public void Generate(IWorldGenerator generator)
    {
        chunkManager.Generate(generator);
    }
}
