public class DebugWorldGenerator : IWorldGenerator
{
    public TileScriptableObject[] Generate(ChunkPos pos)
    {
        TileScriptableObject[] tiles = new TileScriptableObject[ChunkManager.ChunkArea];

        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = TileRegistry.Instance.GetTile(TileIds.Grass);
        }

        return tiles;
    }
}
