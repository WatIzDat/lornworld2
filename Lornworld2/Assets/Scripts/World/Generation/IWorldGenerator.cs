using System;

public interface IWorldGenerator
{
    Func<ChunkPos, ChunkData> GetGenerator(int seed);
}
