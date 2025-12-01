using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MobScriptableObject", menuName = "Scriptable Objects/Mobs/Mob")]
public class MobScriptableObject : ScriptableObject
{
    public string mobName;
    public GameObject mobPrefab;
    public float maxHealth;
    public float attackDamage;
    public float speed;
    public MobPathfindingBehaviorScriptableObject mobPathfindingBehavior;
    public MobFollowPathBehaviorScriptableObject mobFollowPathBehavior;
    public List<StateTransitionInfo<EnemyState, EnemyTransitionCondition>> stateTransitions;
    public EnemyState initialState;
}
