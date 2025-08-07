using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class AStarPathfinding : MonoBehaviour
{
    private PathfindingGrid grid;
    private Heap<PathfindingNode> openSet;

    private PathRequestManager pathRequestManager;

    private void Awake()
    {
        pathRequestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<PathfindingGrid>();
    }

    private void Start()
    {
        Debug.Log(grid.GridArea);
        openSet = new Heap<PathfindingNode>(grid.GridArea);
    }

    public void StartFindPath(Vector2 startPos, Vector2 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    // optimization: check if target node is unwalkable and return early or first search for walkable node
    private IEnumerator FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Vector2[] waypoints = new Vector2[0];
        bool pathSuccess = false;

        PathfindingNode startNode = grid.GetNodeAtWorldPos(startPos);
        PathfindingNode targetNode = grid.GetNodeAtWorldPos(targetPos);

        startNode.gCost = 0;

        if (startNode.Walkable && targetNode.Walkable)
        {
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
                    pathSuccess = true;

                    break;
                }

                // optimization: create a list of neighbors for each Node when creating the grid to skip this entire step
                // TODO: prevent pathfinding through corners
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

        yield return null;

        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }

        pathRequestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    private Vector2[] RetracePath(PathfindingNode startNode, PathfindingNode targetNode)
    {
        List<PathfindingNode> path = new();

        PathfindingNode currentNode = targetNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);

            currentNode = currentNode.parent;
        }

        Vector2[] waypoints = SimplifyPath(path);

        Array.Reverse(waypoints);

        return waypoints;
    }

    private Vector2[] SimplifyPath(List<PathfindingNode> path)
    {
        List<Vector2> waypoints = new();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new(
                path[i - 1].GridX - path[i].GridX,
                path[i - 1].GridY - path[i].GridY);

            if (directionNew != directionOld)
            {
                waypoints.Add(path[i - 1].TilePos);
            }

            directionOld = directionNew;
        }

        return waypoints.ToArray();
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
