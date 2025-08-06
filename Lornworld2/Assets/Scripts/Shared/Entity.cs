using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public ChunkPos ChunkPos => ChunkUtil.ToChunkPos(transform.position);

    [SerializeField]
    protected int health;

    [SerializeField]
    protected int maxHealth;
}
