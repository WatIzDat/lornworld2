using System;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    private static PathRequestManager instance;

    private readonly Queue<PathRequest> pathRequests = new();
    private PathRequest currentPathRequest;

    private AStarPathfinding pathfinding;

    private bool isProcessingPath;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);

            Debug.LogError("Can't create another PathRequestManager instance");
        }
        else
        {
            instance = this;
        }

        pathfinding = GetComponent<AStarPathfinding>();
    }

    public static void RequestPath(Vector2 pathStart, Vector2 pathEnd, Action<Vector2[], bool> callback)
    {
        PathRequest newRequest = new(pathStart, pathEnd, callback);

        instance.pathRequests.Enqueue(newRequest);

        instance.TryProcessNext();
    }

    private void TryProcessNext()
    {
        if (!isProcessingPath && pathRequests.Count > 0)
        {
            currentPathRequest = pathRequests.Dequeue();

            isProcessingPath = true;

            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Vector2[] path, bool success)
    {
        currentPathRequest.callback(path, success);

        isProcessingPath = false;

        TryProcessNext();
    }

    private struct PathRequest
    {
        public Vector2 pathStart;
        public Vector2 pathEnd;
        public Action<Vector2[], bool> callback;

        public PathRequest(Vector2 pathStart, Vector2 pathEnd, Action<Vector2[], bool> callback)
        {
            this.pathStart = pathStart;
            this.pathEnd = pathEnd;
            this.callback = callback;
        }
    }
}
