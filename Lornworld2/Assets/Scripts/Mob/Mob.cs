using UnityEngine;
using System.Collections.Generic;

public class Mob : Entity
{
    [HideInInspector]
    public Player player;

    public float AttackDamage { get; private set; }

    private List<StateTransitionInfo<EnemyState, EnemyTransitionCondition>> stateTransitions;
    private EnemyState initialState;

    private StateMachine stateMachine;
    private PathfindingUnit pathfindingUnit;

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

        mob.AttackDamage = mobScriptableObject.attackDamage;

        mob.player = player;

        mob.stateTransitions = mobScriptableObject.stateTransitions;
        mob.initialState = mobScriptableObject.initialState;

        mob.pathfindingUnit = mob.GetComponent<PathfindingUnit>();

        //Debug.Log(mobScriptableObject.speed);
        mob.pathfindingUnit.speed = mobScriptableObject.speed;

        //pathfindingUnit.target = player.transform;
        mob.pathfindingUnit.pathfindingBehavior = mobScriptableObject.mobPathfindingBehavior;
        mob.pathfindingUnit.followPathBehavior = mobScriptableObject.mobFollowPathBehavior;

        return mob;
    }

    private void Start()
    {
        stateMachine = new StateMachine();

        //PatrolState patrolState = new();
        //ChaseState chaseState = new(player.transform, pathfindingUnit);
        //AttackState attackState = new();

        //stateMachine.AddTransition(patrolState, chaseState, playerInChaseRange);
        //stateMachine.AddTransition(chaseState, attackState, playerInAttackRange);

        //stateMachine.AddTransition(chaseState, patrolState, () => !playerInChaseRange());

        //stateMachine.AddTransition(attackState, chaseState, () => !playerInAttackRange());

        //stateMachine.AddAnyTransition(attackState, playerInAttackRange);

        // TODO: Implement any transitions
        foreach (StateTransitionInfo<EnemyState, EnemyTransitionCondition> transitionInfo in stateTransitions)
        {
            Debug.Log("test");

            EnemyState fromState = Instantiate(transitionInfo.fromState);
            EnemyState toState = Instantiate(transitionInfo.toState);
            EnemyTransitionCondition transitionCondition = Instantiate(transitionInfo.condition);

            fromState.Initialize(this);
            toState.Initialize(this);
            transitionCondition.Initialize(this);

            stateMachine.AddTransition(
                fromState,
                toState,
                transitionCondition.Condition);

            if (transitionInfo.includeInverse)
            {
                stateMachine.AddTransition(
                    toState,
                    fromState,
                    transitionCondition.InverseCondition);
            }
        }

        EnemyState initialStateClone = Instantiate(initialState);
        initialStateClone.Initialize(this);

        stateMachine.SetState(initialStateClone);

        //bool playerInChaseRange()
        //    => Vector2.Distance(transform.position, player.transform.position) < 10f;
    }

    private void Update()
    {
        stateMachine.Tick();
    }

    protected override void OnDeath()
    {
        Debug.Log("mob death");

        Destroy(gameObject);
    }
}
