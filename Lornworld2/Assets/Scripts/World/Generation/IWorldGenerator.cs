using UnityEngine.Tilemaps;

public interface IWorldGenerator
{
    // make the tile array into an actual data structure
    void Generate(ChunkPos pos, Tile[] tiles);
}
