using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public ChunkPos ChunkPos => ChunkUtil.ToChunkPos(transform.position);

    public bool MovedThisFrame { get; private set; }

    [SerializeField]
    protected float health;

    [SerializeField]
    protected float baseHealth;

    protected float maxHealth;

    private Vector2 prevPosition;

    private void Update()
    {
        MovedThisFrame = (Vector2)transform.position != prevPosition;

        prevPosition = transform.position;
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            OnDeath();

            return;
        }

        Debug.Log(health);
    }

    protected abstract void OnDeath();
}
