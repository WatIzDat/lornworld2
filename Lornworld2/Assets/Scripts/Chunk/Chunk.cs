using MemoryPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk : MonoBehaviour, IDataPersistence<ChunkDataPersistence>
{
    [SerializeField]
    private GameObject featurePrefab;

    // array is bottom left to top right counting left to right
    private readonly ObservableCollection<TileScriptableObject> tiles = new();

    public readonly List<Feature> features = new(); 

    private DualGridTilemap tilemap;

    private TilemapRenderer displayTilemapRenderer;

    public ChunkPos chunkPos;

    public static event Action<ChunkPos, int, TileScriptableObject> ChunkChanged;

    //public event Action SaveRequested;
    //public event Func<bool> LoadRequested;

    private ChunkManager chunkManager;

    //public Chunk(ChunkPos chunkPos)
    //{
    //    this.chunkPos = chunkPos;

    //    tiles.CollectionChanged += OnTilesChanged;
    //}

    private void OnEnable()
    {
        DataPersistenceManager.SaveTriggered += SaveData;
    }

    private void OnDisable()
    {
        DataPersistenceManager.SaveTriggered -= SaveData;
    }

    public static Chunk Create(ChunkPos chunkPos, GameObject chunkPrefab, Transform chunkParent, ChunkManager chunkManager)
    {
        Chunk chunk = 
            Instantiate(
                chunkPrefab,
                new Vector3(chunkPos.pos.x, chunkPos.pos.y, 0) * ChunkManager.ChunkSize,
                Quaternion.identity,
                chunkParent)
            .GetComponent<Chunk>();

        chunk.chunkManager = chunkManager;

        chunk.tilemap.chunkManager = chunkManager;
        chunk.tilemap.chunk = chunk;

        chunk.chunkPos = chunkPos;

        chunk.tiles.CollectionChanged += chunk.OnTilesChanged;

        return chunk;
    }

    public static void Pool(ChunkPos chunkPos, GameObject unusedChunk, Action<Chunk> callback)
    {
        unusedChunk.transform.position = new Vector3(chunkPos.pos.x, chunkPos.pos.y, 0) * ChunkManager.ChunkSize;
        unusedChunk.SetActive(true);
        //unusedChunk.transform.GetChild(0).GetChild(1).GetComponent<TilemapRenderer>().enabled = true;

        Chunk chunk = unusedChunk.GetComponent<Chunk>();

        //chunk.SaveRequested?.Invoke();
        DataPersistenceManager.Instance.SaveObject(chunk.SaveData, () =>
        {
            chunk.chunkPos = chunkPos;

            chunk.tilemap.ClearAllTiles();
            chunk.tiles.Clear();

            foreach (Feature feature in chunk.features)
            {
                if (feature != null)
                {
                    Destroy(feature.gameObject);
                }
            }

            chunk.features.Clear();

            callback(chunk);
        });

        //return chunk;
    }

    public TileScriptableObject GetTile(Vector2Int pos)
    {
        return tilemap.GetWorldTile(pos);
    }

    private void Awake()
    {
        tilemap = GetComponentInChildren<DualGridTilemap>();

        displayTilemapRenderer = tilemap.transform.GetChild(1).GetComponent<TilemapRenderer>();
    }

    private void OnTilesChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        //if (e.NewItems == null)
        //{
        //    return;
        //}

        //foreach (TileScriptableObject tile in e.NewItems)
        //{
        //    Vector2Int pos = new(
        //        e.NewStartingIndex % ChunkManager.ChunkSize,
        //        e.NewStartingIndex / ChunkManager.ChunkSize);

        //    // TODO: use SetTileBlock instead of SetTile for performance
        //    tilemap.SetTile(pos, tile);

        //    ChunkChanged?.Invoke(chunkPos, e.NewStartingIndex, tile);
        //}
    }

    public void PopulateWith(Func<ChunkPos, ChunkData> generate, Action callback)
    {
        BoundsInt bounds = new(0, 0, 0, ChunkManager.ChunkSize, ChunkManager.ChunkSize, 1);

        DataPersistenceManager.Instance.LoadObject<ChunkDataPersistence>(
            data =>
            {
                LoadData(data);

                Debug.Log(tiles.Count);
                tilemap.SetWorldTilesBlock(bounds, tiles.ToArray());

                callback();

                //return tiles.ToArray();
            },
            () =>
            {
                ChunkData generatedChunk = generate(chunkPos);

                tiles.AddRange(generatedChunk.tiles);

                foreach ((FeatureScriptableObject feature, Vector2 pos, FeatureData data) feature in generatedChunk.features)
                {
                    Feature.Create(
                        this,
                        feature.feature,
                        feature.pos,
                        feature.data);
                }

                //BoundsInt bounds = new(0, 0, 0, ChunkManager.ChunkSize, ChunkManager.ChunkSize, 1);

                Debug.Log(generatedChunk.tiles.Count());

                tilemap.SetWorldTilesBlock(bounds, generatedChunk.tiles);

                callback();

                //return generatedChunk.tiles;
            },
            Path.Combine(ScenePersistentInfo.SceneId, chunkPos.pos.ToString()));
    }

    //public void PopulateAndSetDisplayTilesWith(Func<ChunkPos, ChunkData> generate)
    //{
    //    PopulateWith(generate);

    //    SetDisplayTiles();
    //}

    public void SetDisplayTiles()
    {
        BoundsInt bounds = new(0, 0, 0, ChunkManager.ChunkSize, ChunkManager.ChunkSize, 1);

        tilemap.SetDisplayTilesBlock(bounds, tiles.ToArray());
    }

    public void SetDisplayOrder(int displayOrder)
    {
        displayTilemapRenderer.sortingOrder = displayOrder;
    }

    public bool LoadData(ChunkDataPersistence data)
    {
        //if (!data.chunkFilePaths.ContainsKey(chunkPos))
        //{
        //    return false;
        //}

        //if (data == null)
        //{
        //    return false;
        //}

        tiles.Clear();
        tiles.AddRange(data.tiles.Select(t => TileRegistry.Instance.GetEntry(t)));

        //Debug.Log(features.Count);
        //features.Clear();
        Debug.Log("Loaded: " + data.features.Count());
        //data.features.Select(f =>
        //    Feature.Create(
        //        this,
        //        FeatureRegistry.Instance.GetEntry(f.feature),
        //        f.pos,
        //        new EmptyFeatureData()));

        foreach ((FeatureIdentifier feature, Vector2 pos, FeatureData data) feature in data.features)
        {
            Feature.Create(
                this,
                FeatureRegistry.Instance.GetEntry(feature.feature),
                feature.pos,
                feature.data);
        }

        return true;
    }

    public void SaveData(Action<IGameData, string> saveCallback)
    {
        //data.chunkFilePaths[chunkPos] = new ChunkDataPersistence(
        //    tiles
        //        .Select(t => TileRegistry.Instance.GetId(t))
        //        .ToArray(),
        //    features
        //        .Where(f => f != null)
        //        .Select(f => (FeatureRegistry.Instance.GetId(f.FeatureScriptableObject), (Vector2)f.transform.localPosition))
        //        .ToArray());

        Debug.Log("Saved: " + features.Count);
        ChunkDataPersistence chunkData = new(
            tiles
                .Select(t => TileRegistry.Instance.GetId(t))
                .ToArray(),
            features
                .Where(f => f != null)
                .Select(f => (FeatureRegistry.Instance.GetId(f.FeatureScriptableObject), (Vector2)f.transform.localPosition, f.data))
                .ToArray());

        saveCallback(chunkData, Path.Combine(ScenePersistentInfo.SceneId, chunkPos.pos.ToString()));
    }
}
