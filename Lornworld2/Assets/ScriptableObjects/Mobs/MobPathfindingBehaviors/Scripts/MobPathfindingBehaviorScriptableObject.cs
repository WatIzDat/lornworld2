using System;
using UnityEngine;

public abstract class MobPathfindingBehaviorScriptableObject : ScriptableObject, IMobPathfindingBehavior
{
    public abstract void RequestPath(Vector2 pathStart, Vector2 pathEnd, Action<Vector2[], bool> callback);
}
