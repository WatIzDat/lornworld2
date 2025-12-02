using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MobManager : MonoBehaviour
{
    //[SerializeField]
    //private GameObject mobPrefab;

    [SerializeField]
    private string mobParentTag;

    //[SerializeField]
    private Transform mobParent;

    [SerializeField]
    private Player player;

    private ChunkManager chunkManager;

    [SerializeField]
    private int maxMobCount;

    [SerializeField]
    private int minMobSpawnChunkDistance;

    private bool isSpawningMob;

    //private void Start()
    //{
    //SpawnMob(MobRegistry.Instance.GetEntry(MobIds.SlimeMob), new Vector2(20, 30));
    //SpawnMob(MobRegistry.Instance.GetEntry(MobIds.SlimeMob), new Vector2(-10, 15));
    //}

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        mobParent = GameObject.FindGameObjectWithTag(mobParentTag).transform;

        chunkManager = FindFirstObjectByType<ChunkManager>();
    }

    private void Update()
    {
        if (chunkManager.AreInitialChunksGenerated &&
            !chunkManager.IsShiftingChunks &&
            mobParent.childCount < maxMobCount &&
            !isSpawningMob)
        {
            StartCoroutine(TrySpawnMobAtRandomPos());
            //Vector2? spawnpoint = null;

            //while (spawnpoint == null)
            //{
            //    Chunk chunk = chunkManager.GetRandomLoadedChunk();

            //    //Debug.Log("distance: " + Vector2.Distance(chunk.chunkPos.pos, player.ChunkPos.pos));

            //    if (Vector2.Distance(chunk.chunkPos.pos, player.ChunkPos.pos) < minMobSpawnChunkDistance)
            //    {
            //        continue;
            //    }

            //    List<Vector2> spawnableTiles = new();

            //    for (int y = 0; y < ChunkManager.ChunkSize; y++)
            //    {
            //        for (int x = 0; x < ChunkManager.ChunkSize; x++)
            //        {
            //            Vector2Int tilePos = new(x, y);

            //            Vector2 worldPos = tilePos + (chunk.chunkPos.pos * ChunkManager.ChunkSize);

            //            TileScriptableObject tile = chunk.GetTile(tilePos);

            //            if (tile.isWalkable)
            //            {
            //                spawnableTiles.Add(worldPos);
            //            }
            //        }
            //    }

            //    if (spawnableTiles.Count > 0)
            //    {
            //        spawnpoint = spawnableTiles[Random.Range(0, spawnableTiles.Count)];

            //        SpawnMob(MobRegistry.Instance.GetEntry(MobIds.SlimeMob), (Vector2)spawnpoint);
            //    }
            //}
        }

        foreach (Transform mob in mobParent)
        {
            if (Vector2.Distance(mob.position, player.transform.position) >
                (chunkManager.LoadedChunksSideLength * ChunkManager.ChunkSize) / 2)
            {
                Destroy(mob.gameObject);
            }
        }
    }

    private IEnumerator TrySpawnMobAtRandomPos()
    {
        isSpawningMob = true;

        Chunk chunk = chunkManager.GetRandomLoadedChunk();

        //Debug.Log("distance: " + Vector2.Distance(chunk.chunkPos.pos, player.ChunkPos.pos));

        while (Vector2.Distance(chunk.chunkPos.pos, player.ChunkPos.pos) < minMobSpawnChunkDistance)
        {
            yield return null;

            chunk = chunkManager.GetRandomLoadedChunk();
        }

        List<Vector2> spawnableTiles = new();

        for (int y = 0; y < ChunkManager.ChunkSize; y++)
        {
            for (int x = 0; x < ChunkManager.ChunkSize; x++)
            {
                Vector2Int tilePos = new(x, y);

                Vector2 worldPos = tilePos + (chunk.chunkPos.pos * ChunkManager.ChunkSize) + (Vector2.one * 0.5f);

                TileScriptableObject tile = chunk.GetTile(tilePos);

                if (tile.isWalkable)
                {
                    spawnableTiles.Add(worldPos);
                }
            }
        }

        if (spawnableTiles.Count > 0)
        {
            Vector2 spawnpoint = spawnableTiles[Random.Range(0, spawnableTiles.Count)];

            SpawnMob(MobRegistry.Instance.GetEntry(MobIds.SlimeMob), spawnpoint);

            isSpawningMob = false;
        }
        else
        {
            yield return null;
        }
    }

    private void SpawnMob(MobScriptableObject mobScriptableObject, Vector2 position)
    {
        Mob.Create(player, mobParent, mobScriptableObject, position);
    }
}
