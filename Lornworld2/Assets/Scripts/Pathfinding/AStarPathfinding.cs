using UnityEngine;
using System.Collections.Generic;

public class AStarPathfinding : MonoBehaviour
{
    private PathfindingGrid grid;
    private Heap<PathfindingNode> openSet;

    [SerializeField]
    private Transform seeker, target;

    private void Awake()
    {
        grid = GetComponent<PathfindingGrid>();
    }

    private void Start()
    {
        Debug.Log(grid.GridArea);
        openSet = new Heap<PathfindingNode>(grid.GridArea);
    }

    private void Update()
    {
        FindPath(seeker.position, target.position);
    }

    // optimization: check if target node is unwalkable and return early or first search for walkable node
    private void FindPath(Vector2 startPos, Vector2 targetPos)
    {
        PathfindingNode startNode = grid.GetNodeAtWorldPos(startPos);
        PathfindingNode targetNode = grid.GetNodeAtWorldPos(targetPos);

        startNode.gCost = 0;

        openSet.Clear();
        // optimization: use a two dimensional bool array to mark items for the closedSet
        // optimization: use .Clear() instead to avoid too much garbage
        HashSet<PathfindingNode> closedSet = new();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            PathfindingNode currentNode = openSet.RemoveFirst();

            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);

                return;
            }

            // optimization: create a list of neighbors for each Node when creating the grid to skip this entire step
            foreach (PathfindingNode neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.Walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newGCost = currentNode.gCost + GetDistance(currentNode, neighbour);

                if (newGCost < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newGCost;
                    neighbour.hCost = GetDistance(neighbour, targetNode);

                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                    else
                    {
                        openSet.UpdateItem(neighbour);
                    }
                }
            }
        }
    }

    private void RetracePath(PathfindingNode startNode, PathfindingNode targetNode)
    {
        List<PathfindingNode> path = new();

        PathfindingNode currentNode = targetNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);

            currentNode = currentNode.parent;
        }

        path.Reverse();

        grid.path = path;
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
