using System.Collections.Generic;
using UnityEngine;

public class Feature : Entity
{
    [SerializeField]
    private GameObject droppedItemPrefab;

    public FeatureScriptableObject FeatureScriptableObject { get; private set; }

    public static Feature Create(GameObject featurePrefab, Chunk chunk, FeatureScriptableObject featureScriptableObject, Vector2 position, bool worldPositionStays = false)
    {
        Feature feature = Instantiate(
            featurePrefab,
            position,
            Quaternion.identity).GetComponent<Feature>();

        feature.transform.SetParent(chunk.transform, worldPositionStays);

        feature.MaxHealth = featureScriptableObject.maxHealth;
        feature.baseHealth = feature.MaxHealth;
        feature.Health = feature.MaxHealth;

        feature.FeatureScriptableObject = featureScriptableObject;

        chunk.features.Add(feature);

        return feature;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        Debug.Log(Health);
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
