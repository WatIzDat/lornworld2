using System.Collections.Generic;
using UnityEngine;

public class PathfindingGrid : MonoBehaviour
{
    private Vector2Int gridSize;
    private PathfindingNode[,] grid;

    [SerializeField]
    private ChunkManager chunkManager;

    //[SerializeField]
    //private Player player;

    private void Start()
    {
        transform.position = Vector2.one * (ChunkManager.ChunkSize / 2);

        // TODO: separate render distance from simulation distance
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

                grid[x, y] = new PathfindingNode(walkable, tilePos, x, y);
            }
        }
    }

    public List<PathfindingNode> GetNeighbours(PathfindingNode node)
    {
        List<PathfindingNode> neighbours = new();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int neighbourX = node.GridX + x;
                int neighbourY = node.GridY + y;

                if (neighbourX >= 0 && neighbourX < gridSize.x &&
                    neighbourY >= 0 && neighbourY < gridSize.y)
                {
                    neighbours.Add(grid[neighbourX, neighbourY]);
                }
            }
        }

        return neighbours;
    }

    public PathfindingNode GetNodeAtWorldPos(Vector2 pos)
    {
        float percentX = ((pos.x - (ChunkManager.ChunkSize / 2)) / gridSize.x) + 0.5f;
        float percentY = ((pos.y - (ChunkManager.ChunkSize / 2)) / gridSize.y) + 0.5f;

        int x = Mathf.FloorToInt(Mathf.Clamp(gridSize.x * percentX, 0, gridSize.x - 1));
        int y = Mathf.FloorToInt(Mathf.Clamp(gridSize.y * percentY, 0, gridSize.y - 1));

        return grid[x, y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, gridSize.y, 1));

        if (grid != null)
        {
            //PathfindingNode playerNode = GetNodeAtWorldPos(player.transform.position);

            foreach (PathfindingNode node in grid)
            {
                Gizmos.color = node.Walkable ? Color.white : Color.red;

                //if (playerNode == node)
                //{
                //    Gizmos.color = Color.cyan;
                //}

                Gizmos.DrawCube(new Vector2(node.TilePos.x + 0.5f, node.TilePos.y + 0.5f), Vector3.one * 0.9f);
            }
        }
    }
}
