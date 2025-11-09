using UnityEngine;

public class PlayerBuild : MonoBehaviour
{
    [SerializeField]
    private ChunkManager chunkManager;

    [SerializeField]
    private GameObject featurePrefab;

    private PlayerInventory playerInventory;

    private FeatureScriptableObject featureToPlace;
    private bool placeFeatureNextUpdate;

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

    private void FixedUpdate()
    {
        if (placeFeatureNextUpdate)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D raycastHit = Physics2D.Raycast(pos, Vector2.zero);

            if (raycastHit.collider != null)
            {
                placeFeatureNextUpdate = false;

                return;
            }

            Chunk chunk = chunkManager.FindChunkAt(new ChunkPos(Vector2Int.FloorToInt(pos / ChunkManager.ChunkSize)));

            Feature.Create(featurePrefab, chunk, featureToPlace, pos, true);

            placeFeatureNextUpdate = false;
        }
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
        featureToPlace = feature;

        placeFeatureNextUpdate = true;

        playerInventory.RemoveFromSelectedItem();
    }
}
