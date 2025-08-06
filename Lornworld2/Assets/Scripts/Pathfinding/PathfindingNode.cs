using UnityEngine;

public class PathfindingNode
{
    public bool Walkable { get; }
    public Vector2Int TilePos { get; }

    public PathfindingNode(bool walkable, Vector2Int tilePos)
    {
        Walkable = walkable;
        TilePos = tilePos;
    }
}
