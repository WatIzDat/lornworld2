using System.Collections;
using UnityEngine;

public class PathfindingUnit : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private Rigidbody2D rb;

    private float speed = 0.01f;
    private Vector2[] path;
    private int targetIndex;

    private Coroutine followPathCoroutine;

    private bool pathRequested;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!pathRequested)
        {
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);

            pathRequested = true;
        }
    }

    private void OnPathFound(Vector2[] newPath, bool success)
    {
        if (success)
        {
            path = newPath;

            if (followPathCoroutine != null)
            {
                StopCoroutine(followPathCoroutine);
            }

            followPathCoroutine = StartCoroutine(FollowPath());
        }
    }

    private IEnumerator FollowPath()
    {
        Vector2 currentWaypoint = path[0];

        while (true)
        {
            if ((Vector2)transform.position == currentWaypoint)
            {
                targetIndex++;

                if (targetIndex >= path.Length)
                {
                    yield break;
                }

                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, speed);

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;

                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
