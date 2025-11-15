using UnityEngine;

public abstract class WorldGeneratorScriptableObject : ScriptableObject, IWorldGenerator
{
    public abstract ChunkData Generate(ChunkPos pos);
}
