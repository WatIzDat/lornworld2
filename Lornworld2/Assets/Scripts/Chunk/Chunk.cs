using System;
using UnityEngine.Tilemaps;

public class Chunk
{
    private Tile[] tiles;

    public ChunkPos chunkPos;

    public Chunk(ChunkPos chunkPos)
    {
        this.chunkPos = chunkPos;
    }

    public void PopulateWith(Func<ChunkPos, Tile[], Tile[]> generate)
    {
    }
}
