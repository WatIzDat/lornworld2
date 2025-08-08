using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public ChunkPos ChunkPos => ChunkUtil.ToChunkPos(transform.position);

    [SerializeField]
    protected float health;

    [SerializeField]
    protected float maxHealth;
}
