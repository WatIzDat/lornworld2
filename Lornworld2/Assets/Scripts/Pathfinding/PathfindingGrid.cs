using UnityEngine;

public class PathfindingGrid : MonoBehaviour
{
    private Vector2Int gridSize;
    private PathfindingNode[,] grid;

    [SerializeField]
    private ChunkManager chunkManager;

    private void Start()
    {
        transform.position = Vector2.one * (ChunkManager.ChunkSize / 2);

        gridSize = ChunkManager.ChunkSize * chunkManager.LoadedChunksSideLength * Vector2Int.one;
    }
    public void Initialize()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new PathfindingNode[gridSize.x, gridSize.y];

        Vector2Int bottomLeft = new(
            (int)transform.position.x - (gridSize.x / 2),
            (int)transform.position.y - (gridSize.y / 2));

        Debug.Log(bottomLeft);

        for (int x = 0; x < gridSize.x; x++)
        { 
            for (int y = 0; y < gridSize.x; y++)
            {
                Vector2Int tilePos = new(bottomLeft.x + x, bottomLeft.y + y);

                ChunkPos chunkPos = ChunkUtil.ToChunkPos(tilePos);

                Vector2Int relativeTilePos = new(
                        MathHelper.MathematicalMod(tilePos.x, ChunkManager.ChunkSize),
                        MathHelper.MathematicalMod(tilePos.y, ChunkManager.ChunkSize));

                TileScriptableObject tile = chunkManager.GetTileInChunkAt(
                    chunkPos, relativeTilePos);

                Debug.Log(tile + "  " + chunkPos + "  " + relativeTilePos);

                // add an isWalkable field to tile scriptable objects
                bool walkable = tile != TileRegistry.Instance.GetEntry(TileIds.WaterTile);

                grid[x, y] = new PathfindingNode(walkable, tilePos);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, gridSize.y, 1));

        if (grid != null)
        {
            foreach (PathfindingNode node in grid)
            {
                Gizmos.color = node.Walkable ? Color.white : Color.red;

                Gizmos.DrawCube(new Vector2(node.TilePos.x + 0.5f, node.TilePos.y + 0.5f), Vector3.one * 0.9f);
            }
        }
    }
}
