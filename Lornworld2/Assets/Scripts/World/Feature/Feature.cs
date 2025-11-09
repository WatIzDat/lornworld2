using UnityEngine;

public class Feature : Entity
{
    [SerializeField]
    private GameObject droppedItemPrefab;

    private InventoryItem[] itemDrops;

    public static Feature Create(GameObject featurePrefab, Chunk chunk, FeatureScriptableObject featureScriptableObject, Vector2 position)
    {
        Feature feature = Instantiate(
            featurePrefab,
            position,
            Quaternion.identity).GetComponent<Feature>();

        feature.transform.SetParent(chunk.transform, false);

        feature.MaxHealth = featureScriptableObject.maxHealth;
        feature.baseHealth = feature.MaxHealth;
        feature.Health = feature.MaxHealth;

        feature.itemDrops = featureScriptableObject.itemDrops;

        return feature;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        Debug.Log(Health);
    }

    protected override void OnDeath()
    {
        foreach (InventoryItem inventoryItem in itemDrops)
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
