using System;
using UnityEngine;

public interface IMobPathfindingBehavior
{
    void RequestPath(Vector2 pathStart, Vector2 pathEnd, Action<Vector2[], bool> callback);
}
