public class DebugWorldGenerator : IWorldGenerator
{
    public TileScriptableObject[] Generate(ChunkPos pos)
    {
        TileScriptableObject[] tiles = new TileScriptableObject[ChunkManager.ChunkArea];

        TileScriptableObject tile = TileRegistry.Instance.GetTile(TileIds.Water);

        // checkerboard pattern, if both coordinates are even or both coordinates are odd set to grass
        if ((pos.pos.x % 2 == 0 && pos.pos.y % 2 == 0) ||
            (pos.pos.x % 2 != 0 && pos.pos.y % 2 != 0))
        {
            tile = TileRegistry.Instance.GetTile(TileIds.Grass);
        }

        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = tile;
        }

        return tiles;
    }
}
