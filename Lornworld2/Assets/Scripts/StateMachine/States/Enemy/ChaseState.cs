using UnityEngine;

public class ChaseState : IState
{
    private readonly Transform target;
    private readonly PathfindingUnit pathfindingUnit;

    public ChaseState(Transform target, PathfindingUnit pathfindingUnit)
    {
        this.target = target;
        this.pathfindingUnit = pathfindingUnit;
    }

    public void OnEnter()
    {
        pathfindingUnit.RequestFollowPath(target);
    }

    public void OnExit()
    {
        pathfindingUnit.StopFollowPath();
    }

    public void Tick()
    {
    }
}
