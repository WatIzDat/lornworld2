using UnityEngine;

[CreateAssetMenu(fileName = "DropItemListFeatureDeathBehavior", menuName = "Scriptable Objects/World/Features/DeathBehaviors/DropItemList")]
public class DropItemListFeatureDeathBehavior : BaseDropItemFeatureDeathBehavior<FeatureData>
{
    [SerializeField]
    private InventoryItem[] itemDrops;

    public override void Die(FeatureData data, Vector2 deathPos)
    {
        foreach (InventoryItem item in itemDrops)
        {
            DropItem(deathPos, item);
        }
    }
}
