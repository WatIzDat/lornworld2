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
        SpawnMob(MobRegistry.Instance.GetEntry(MobIds.SlimeMob), Vector2.one);
    }

    private void SpawnMob(MobScriptableObject mobScriptableObject, Vector2 position)
    {
        Mob.Create(player, mobPrefab, mobParent, mobScriptableObject, position);
    }
}
