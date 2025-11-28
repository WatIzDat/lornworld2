using UnityEngine;

[CreateAssetMenu(fileName = "OreFeatureDeathBehavior", menuName = "Scriptable Objects/World/Features/DeathBehaviors/Ore")]
public class OreFeatureDeathBehavior : BaseDropItemFeatureDeathBehavior<OreFeatureData>
{
    [SerializeField]
    private OreTypeToItemDropsMap[] oreTypeToItemDropsMap;

    [System.Serializable]
    private class OreTypeToItemDropsMap
    {
        public OreType oreType;
        public InventoryItem[] itemDrops;
    }

    public override void Die(FeatureData data, Vector2 deathPos)
    {
        base.Die(data, deathPos);

        OreFeatureData oreFeatureData = (OreFeatureData)data;

        foreach (OreTypeToItemDropsMap map in oreTypeToItemDropsMap)
        {
            if (map.oreType == oreFeatureData.oreType)
            {
                foreach (InventoryItem item in map.itemDrops)
                {
                    DropItem(deathPos, item);
                }
            }
        }
    }
}
