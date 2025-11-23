using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public ChunkPos ChunkPos => ChunkUtil.ToChunkPos(transform.position);

    public bool MovedThisFrame { get; private set; }

    public float Health { get; protected set; }

    [SerializeField]
    protected float baseHealth;

    [field: SerializeField]
    public float MaxHealth { get; protected set; }

    [SerializeField]
    private bool isInvulnerable;

    private Vector2 prevPosition;

    protected virtual void Update()
    {
        MovedThisFrame = (Vector2)transform.position != prevPosition;

        prevPosition = transform.position;
    }

    public virtual void TakeDamage(float damage)
    {
        if (isInvulnerable)
        {
            return;
        }

        Health -= damage;

        if (Health <= 0)
        {
            OnDeath();

            return;
        }

        Debug.Log(Health);
    }

    protected abstract void OnDeath();
}
