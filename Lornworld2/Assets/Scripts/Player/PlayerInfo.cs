using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public ChunkPos ChunkPos => ChunkUtil.ToChunkPos(transform.position);
}
