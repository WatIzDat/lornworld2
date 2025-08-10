using UnityEngine;

public class MobManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mobPrefab;

    [SerializeField]
    private Transform mobParent;

    [SerializeField]
    private Player player;

    private void Start()
    {
        SpawnMob(MobRegistry.Instance.GetEntry(MobIds.SlimeMob), new Vector2(20, 30));
        SpawnMob(MobRegistry.Instance.GetEntry(MobIds.SlimeMob), new Vector2(-10, 15));
    }

    private void SpawnMob(MobScriptableObject mobScriptableObject, Vector2 position)
    {
        Mob.Create(player, mobPrefab, mobParent, mobScriptableObject, position);
    }
}
