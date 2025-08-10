using UnityEngine;

public class Mob : Entity
{
    private StateMachine stateMachine;
    private PathfindingUnit pathfindingUnit;
    private Player player;

    // TODO: implement state machine and get rid of directly passing in player
    public static Mob Create(Player player, GameObject mobPrefab, Transform mobParent, MobScriptableObject mobScriptableObject, Vector2 position)
    {
        Mob mob = Instantiate(
            mobPrefab,
            position,
            Quaternion.identity,
            mobParent).GetComponent<Mob>();

        mob.maxHealth = mobScriptableObject.maxHealth;
        mob.health = mob.maxHealth;

        mob.player = player;

        mob.pathfindingUnit = mob.GetComponent<PathfindingUnit>();

        //pathfindingUnit.target = player.transform;
        mob.pathfindingUnit.pathfindingBehavior = mobScriptableObject.mobPathfindingBehavior;
        mob.pathfindingUnit.followPathBehavior = mobScriptableObject.mobFollowPathBehavior;

        return mob;
    }

    private void Start()
    {
        stateMachine = new StateMachine();

        PatrolState patrolState = new();
        ChaseState chaseState = new(player.transform, pathfindingUnit);
        //AttackState attackState = new();

        stateMachine.AddTransition(patrolState, chaseState, playerInChaseRange);
        //stateMachine.AddTransition(chaseState, attackState, playerInAttackRange);

        stateMachine.AddTransition(chaseState, patrolState, () => !playerInChaseRange());

        stateMachine.SetState(patrolState);
        //stateMachine.AddTransition(attackState, chaseState, () => !playerInAttackRange());

        //stateMachine.AddAnyTransition(attackState, playerInAttackRange);

        bool playerInChaseRange() 
            => Vector2.Distance(transform.position, player.transform.position) < 10f;
    }

    private void Update()
    {
        stateMachine.Tick();
    }
}
