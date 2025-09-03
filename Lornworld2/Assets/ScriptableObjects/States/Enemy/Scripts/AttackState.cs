using UnityEngine;

[CreateAssetMenu(fileName = "AttackState", menuName = "Scriptable Objects/States/Enemy/Attack")]
public class AttackState : EnemyState
{
    [SerializeField]
    private float attackCooldown;

    private float cooldownTimer;

    private Entity target;
    private float damage;

    public override void Initialize(Mob mob)
    {
        base.Initialize(mob);

        target = mob.player.GetComponent<Entity>();
        damage = mob.AttackDamage;
    }

    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }

    public override void Tick()
    {
        if (cooldownTimer <= 0f)
        {
            target.TakeDamage(damage);

            cooldownTimer = attackCooldown;
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
        }
    }
}
