using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField]
    private ChunkManager chunkManager;

    public void Generate(IWorldGenerator generator)
    {
        chunkManager.Generate(generator);
    }
}
