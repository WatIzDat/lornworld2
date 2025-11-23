using UnityEngine;

//public class Feature : Feature<EmptyFeatureData>
//{
//}

//public interface IFeature
//{
//    FeatureScriptableObject<FeatureData> FeatureScriptableObject { get; set; }
//}

public class Feature : Entity
{
    [SerializeField]
    private GameObject droppedItemPrefab;

    //public FeatureScriptableObject FeatureScriptableObject { get; set; }

    public FeatureData data;

    public FeatureScriptableObject FeatureScriptableObject { get; protected set; }

    public static Feature Create(Chunk chunk, FeatureScriptableObject featureScriptableObject, Vector2 position, FeatureData data = null, bool worldPositionStays = false)
    {
        Feature feature = Instantiate(
            featureScriptableObject.prefab,
            position,
            Quaternion.identity).GetComponent<Feature>();

        feature.transform.SetParent(chunk.transform, worldPositionStays);

        feature.MaxHealth = featureScriptableObject.maxHealth;
        feature.baseHealth = feature.MaxHealth;
        feature.Health = feature.MaxHealth;

        feature.FeatureScriptableObject = featureScriptableObject;

        if (data != null)
        {
            if (featureScriptableObject.featureInitBehavior == null)
            {
                feature.data = data;
            }
            else
            {
                featureScriptableObject.featureInitBehavior.Init(feature, data);
            }
        }

        chunk.features.Add(feature);

        return feature;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        Debug.Log(Health);

        FeatureScriptableObject.featureHitBehavior.Hit(data);
    }

    protected override void OnDeath()
    {
        foreach (InventoryItem inventoryItem in FeatureScriptableObject.itemDrops)
        {
            DroppedItem.Create(
                droppedItemPrefab,
                transform.position,
                inventoryItem.item,
                inventoryItem.stackSize);
        }

        Destroy(gameObject);
    }
}
