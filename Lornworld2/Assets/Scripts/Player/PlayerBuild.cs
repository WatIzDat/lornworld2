using UnityEngine;

public class PlayerBuild : MonoBehaviour
{
    private PlayerInventory playerInventory;

    private FeatureItemUseBehavior prevFeatureItemUseBehavior;

    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventory>();
    }

    private void OnEnable()
    {
        PlayerInventory.HotbarSelectedIndexChanged += OnHotbarSelectedIndexChanged;
    }

    private void OnDisable()
    {
        PlayerInventory.HotbarSelectedIndexChanged -= OnHotbarSelectedIndexChanged;
    }

    private void OnHotbarSelectedIndexChanged(int obj)
    {
        if (prevFeatureItemUseBehavior != null)
        {
            prevFeatureItemUseBehavior.ItemUsed -= PlaceFeature;

            prevFeatureItemUseBehavior = null;
        }

        if (playerInventory.SelectedItem == null)
        {
            return;
        }

        if (playerInventory.SelectedItem.item.itemUseBehavior is FeatureItemUseBehavior featureItemUseBehavior)
        {
            featureItemUseBehavior.ItemUsed += PlaceFeature;

            prevFeatureItemUseBehavior = featureItemUseBehavior;
        }
    }

    public void PlaceFeature(FeatureScriptableObject feature)
    {
        Debug.Log(feature);
    }
}
