public interface IWorldGenerator
{
    TileScriptableObject[] Generate(ChunkPos pos);
}
