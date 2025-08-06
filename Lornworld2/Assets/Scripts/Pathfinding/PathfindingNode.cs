using UnityEngine;

public class PathfindingNode
{
    public bool Walkable { get; }
    public Vector2Int TilePos { get; }
    public int GridX { get; }
    public int GridY { get; }

    public int gCost;
    public int hCost;
    public int FCost => gCost + hCost;

    public PathfindingNode parent;

    public PathfindingNode(bool walkable, Vector2Int tilePos, int gridX, int gridY)
    {
        Walkable = walkable;
        TilePos = tilePos;
        GridX = gridX;
        GridY = gridY;
    }
}
