public class DebugWorldGenerator : IWorldGenerator
{
    public TileScriptableObject[] Generate(ChunkPos pos)
    {
        TileScriptableObject[] tiles = new TileScriptableObject[ChunkManager.ChunkArea];

        TileScriptableObject tile = TileRegistry.Instance.GetEntry(TileIds.WaterTile);

        // checkerboard pattern, if both coordinates are even or both coordinates are odd set to grass
        if ((pos.pos.x % 2 == 0 && pos.pos.y % 2 == 0) ||
            (pos.pos.x % 2 != 0 && pos.pos.y % 2 != 0))
        {
            tile = TileRegistry.Instance.GetEntry(TileIds.GrassTile);
        }

        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = tile;
        }

        return tiles;
    }
}
