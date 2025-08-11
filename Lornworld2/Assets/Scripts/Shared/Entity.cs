using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public ChunkPos ChunkPos => ChunkUtil.ToChunkPos(transform.position);

    public bool MovedThisFrame { get; private set; }

    [SerializeField]
    protected float health;

    [SerializeField]
    protected float maxHealth;

    private Vector2 prevPosition;

    private void Update()
    {
        MovedThisFrame = (Vector2)transform.position != prevPosition;

        prevPosition = transform.position;
    }
}
