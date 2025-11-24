using System;
using UnityEngine;

public abstract class WorldGeneratorScriptableObject : ScriptableObject, IWorldGenerator
{
    public abstract Func<ChunkPos, ChunkData> GetGenerator(int seed);
}
