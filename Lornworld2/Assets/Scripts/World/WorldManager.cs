using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField]
    private ChunkManager chunkManager;

    private void Start()
    {
        Generate(new DebugWorldGenerator());
    }

    public void Generate(IWorldGenerator generator)
    {
        chunkManager.Generate(generator);
    }
}
