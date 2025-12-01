using UnityEngine;

public interface IMobFollowPathBehavior
{
    void FollowPath(Rigidbody2D rb, float speed, float time, Vector2 direction, Vector2 startPos, Vector2 targetPos);
}
