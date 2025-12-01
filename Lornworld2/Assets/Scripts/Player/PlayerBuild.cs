using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBuild : MonoBehaviour
{
    //[SerializeField]
    private ChunkManager chunkManager;

    [SerializeField]
    private GameObject featurePrefab;

    private PlayerInventory playerInventory;

    //private FeatureScriptableObject featureToPlace;
    //private bool placeFeatureNextUpdate;

    private FeatureItemUseBehavior prevFeatureItemUseBehavior;

    private readonly Queue<(Vector2 pos, FeatureScriptableObject feature, int index)> featuresToPlace = new();

    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventory>();
    }

    private void OnEnable()
    {
        PlayerInventory.HotbarSelectedItemChanged += OnHotbarSelectedItemChanged;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        PlayerInventory.HotbarSelectedItemChanged -= OnHotbarSelectedItemChanged;

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        chunkManager = FindFirstObjectByType<ChunkManager>();
    }

    private void FixedUpdate()
    {
        if (featuresToPlace.Count > 0)
        {
            (Vector2 mousePos, FeatureScriptableObject featureToPlace, int index) = featuresToPlace.Dequeue();

            Vector2 pos = Camera.main.ScreenToWorldPoint(mousePos);

            RaycastHit2D raycastHit = Physics2D.Raycast(pos, Vector2.zero);

            if (raycastHit.collider != null)
            {
                //placeFeatureNextUpdate = false;

                return;
            }

            playerInventory.RemoveFromIndex(index);

            StartCoroutine(ProcessFeaturePlacement(pos, featureToPlace));

            //Chunk chunk = chunkManager.FindChunkAt(new ChunkPos(Vector2Int.FloorToInt(pos / ChunkManager.ChunkSize)));

            //Feature.Create(chunk, featureToPlace, pos, worldPositionStays: true);

            //placeFeatureNextUpdate = false;
        }
    }

    private IEnumerator ProcessFeaturePlacement(Vector2 pos, FeatureScriptableObject featureToPlace)
    {
        if (chunkManager.IsShiftingChunks)
        {
            yield return null;
        }

        Chunk chunk = chunkManager.FindChunkAt(new ChunkPos(Vector2Int.FloorToInt(pos / ChunkManager.ChunkSize)));

        Feature.Create(chunk, featureToPlace, pos, worldPositionStays: true);
    }

    private void OnHotbarSelectedItemChanged(int index, InventoryItem oldInventoryItem, InventoryItem newInventoryItem)
    {
        if (prevFeatureItemUseBehavior != null)
        {
            prevFeatureItemUseBehavior.ItemUsed -= PlaceFeature;

            prevFeatureItemUseBehavior = null;
        }

        if (newInventoryItem == null)
        {
            return;
        }

        if (newInventoryItem.item.itemUseBehavior is FeatureItemUseBehavior featureItemUseBehavior)
        {
            featureItemUseBehavior.ItemUsed += PlaceFeature;

            prevFeatureItemUseBehavior = featureItemUseBehavior;
        }
    }

    public void PlaceFeature(FeatureScriptableObject feature)
    {
        //featureToPlace = feature;

        //placeFeatureNextUpdate = true;

        featuresToPlace.Enqueue((Input.mousePosition, feature, playerInventory.SelectedIndex));
    }
}
