using UnityEngine;
using System.Collections.Generic;

public class AStarPathfinding : MonoBehaviour
{
    private PathfindingGrid grid;

    private void Awake()
    {
        grid = GetComponent<PathfindingGrid>();
    }

    private void FindPath(Vector2 startPos, Vector2 targetPos)
    {
        PathfindingNode startNode = grid.GetNodeAtWorldPos(startPos);
        PathfindingNode targetNode = grid.GetNodeAtWorldPos(targetPos);

        List<PathfindingNode> openSet = new();
        // optimization: use a two dimensional bool array to mark items for the closedSet
        HashSet<PathfindingNode> closedSet = new();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            PathfindingNode currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost ||
                    (openSet[i].FCost == currentNode.FCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return;
            }

            // optimization: create a list of neighbors for each Node when creating the grid to skip this entire step
            foreach (PathfindingNode neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.Walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }
            }
        }
    }

    private int GetDistance(PathfindingNode nodeA, PathfindingNode nodeB)
    {
        int distX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        int distY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

        if (distX > distY)
        {
            return (14 * distY) + (10 * (distX - distY));
        }

        return (14 * distX) + (10 * (distY - distX));
    }
}
