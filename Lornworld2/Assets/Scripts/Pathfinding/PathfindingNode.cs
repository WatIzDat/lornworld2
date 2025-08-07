using UnityEngine;

public class PathfindingNode : IHeapItem<PathfindingNode>
{
    public bool Walkable { get; }
    public Vector2Int TilePos { get; }
    public int GridX { get; }
    public int GridY { get; }

    public int gCost;
    public int hCost;
    public int FCost => gCost + hCost;

    public int HeapIndex { get; set; }

    public PathfindingNode parent;

    public PathfindingNode(bool walkable, Vector2Int tilePos, int gridX, int gridY)
    {
        Walkable = walkable;
        TilePos = tilePos;
        GridX = gridX;
        GridY = gridY;
    }

    public int CompareTo(PathfindingNode other)
    {
        int compare = FCost.CompareTo(other.FCost);

        if (compare == 0)
        {
            compare = hCost.CompareTo(other.hCost);
        }

        // lower f/h costs should return 1, higher should return -1, so opposite sign
        return -compare;
    }
}
