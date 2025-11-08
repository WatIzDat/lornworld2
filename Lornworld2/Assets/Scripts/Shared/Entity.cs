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

    private Vector2 prevPosition;

    private void Update()
    {
        MovedThisFrame = (Vector2)transform.position != prevPosition;

        prevPosition = transform.position;
    }

    public virtual void TakeDamage(float damage)
    {
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
