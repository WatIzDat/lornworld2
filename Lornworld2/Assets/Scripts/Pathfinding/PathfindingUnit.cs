using UnityEngine;

public class PathfindingUnit : MonoBehaviour
{
    [HideInInspector]
    public Transform target;

    private Rigidbody2D rb;

    [SerializeField]
    private float speed = 5f;

    private Vector2[] path;
    private int targetIndex;

    private bool followPath;
    private Vector2 currentWaypoint;

    private bool pathRequested;

    private Vector2 startPos;

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

            followPath = true;

            startPos = transform.position;

            currentWaypoint = path[0];
        }
    }

    private void FixedUpdate()
    {
        if (followPath)
        {
            FollowPath();
        }
    }

    private void FollowPath()
    {
        // optimization: use sqrMagnitude and 0.01f to check distances
        if (Vector2.Distance(transform.position, currentWaypoint) < 0.1f)
        {
            targetIndex++;

            if (targetIndex >= path.Length)
            {
                targetIndex = 0;

                path = new Vector2[0];

                followPath = false;

                rb.linearVelocity = Vector2.zero;

                return;
            }

            currentWaypoint = path[targetIndex];
        }

        startPos = targetIndex == 0 ? startPos : path[targetIndex - 1];
        Vector2 targetPos = path[targetIndex];

        rb.linearVelocity = speed * Time.fixedDeltaTime * new Vector2(
            targetPos.x - startPos.x,
            targetPos.y - startPos.y).normalized;
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
