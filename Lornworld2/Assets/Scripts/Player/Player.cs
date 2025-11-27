using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Entity, IDataPersistence<PlayerData>
{
    [SerializeField]
    private float baseAttackDamage;

    public float AttackDamage { get; private set; }

    [SerializeField]
    private int hunger;

    [SerializeField]
    private float regenSpeed;

    private float regenTimer;

    private PlayerInventory playerInventory;

    private bool isNewScene;

    private GameObject currentHeldObject;

    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventory>();

        Health = MaxHealth;
    }

    private void OnEnable()
    {
        //DataPersistenceManager.LoadTriggered += LoadData;
        DataPersistenceManager.SaveTriggered += SaveData;

        SceneManager.sceneLoaded += OnSceneLoaded;
        //SceneManager.activeSceneChanged += OnActiveSceneChanged;

        ChunkManager.InitialChunksGenerated += OnInitialChunksGenerated;

        PlayerInventory.HotbarSelectedItemChanged += OnHotbarSelectedItemChanged;


        InventoryUIManager.ArmorChanged += OnArmorChanged;
    }

    private void OnDisable()
    {
        //DataPersistenceManager.LoadTriggered -= LoadData;
        DataPersistenceManager.SaveTriggered -= SaveData;

        SceneManager.sceneLoaded -= OnSceneLoaded;
        //SceneManager.activeSceneChanged -= OnActiveSceneChanged;

        ChunkManager.InitialChunksGenerated -= OnInitialChunksGenerated;

        PlayerInventory.HotbarSelectedItemChanged -= OnHotbarSelectedItemChanged;

        InventoryUIManager.ArmorChanged -= OnArmorChanged;
    }

    protected override void Update()
    {
        base.Update();

        if (regenTimer <= 0f)
        {
            if (Health + 1f > MaxHealth)
            {
                Health = MaxHealth;
            }
            else
            {
                Health += 1f;
            }

            regenTimer = regenSpeed;
        }

        if (Health < MaxHealth)
        {
            regenTimer -= Time.deltaTime;
        }
        else
        {
            regenTimer = regenSpeed;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log("Scene: " + ScenePersistentInfo.SceneId);

        DataPersistenceManager.Instance.LoadObject<PlayerData>(
            data => LoadData(data),
            () => isNewScene = true,
            Path.Combine(ScenePersistentInfo.SceneId, "player"));
    }

    private void OnInitialChunksGenerated()
    {
        if (isNewScene)
        {
            ChunkManager chunkManager = FindFirstObjectByType<ChunkManager>();
            bool createSceneEntrance = ScenePersistentInfo.SceneId != "surface";

            transform.position = chunkManager.GetSpawnpoint(createSceneEntrance) ?? Vector2.zero;

            isNewScene = false;
        }
    }

    //private void OnActiveSceneChanged(Scene fromScene, Scene toScene)
    //{
    //    DataPersistenceManager.Instance.SaveObject(SaveData, () => { });
    //}

    private void OnArmorChanged(int index, InventoryItem[] items)
    {
        float health = baseHealth;

        foreach (InventoryItem item in items)
        {
            if (item == null || item.item.statScaleBehavior == null)
            {
                continue;
            }

            StatScaleInfo statScaleInfo = item.item.statScaleBehavior.GetStatScaleInfo();

            health += baseHealth * statScaleInfo.HealthScaler;
        }

        MaxHealth = health;

        Debug.Log(MaxHealth);
    }

    private void OnHotbarSelectedItemChanged(int index, InventoryItem oldInventoryItem, InventoryItem newInventoryItem)
    {
        if (oldInventoryItem != null)
        {
            oldInventoryItem.item.itemSelectBehavior.DeselectItem();

            if (oldInventoryItem.item.itemSelectBehavior is InstantiateGameObjectItemSelectBehavior prevInstantiateGameObjectItemSelectBehavior)
            {
                prevInstantiateGameObjectItemSelectBehavior.ItemSelected -= OnItemSelected;
                prevInstantiateGameObjectItemSelectBehavior.ItemDeselected -= OnItemDeselected;
            }
        }

        if (newInventoryItem != null)
        {
            if (newInventoryItem.item.itemSelectBehavior is InstantiateGameObjectItemSelectBehavior instantiateGameObjectItemSelectBehavior)
            {
                instantiateGameObjectItemSelectBehavior.ItemSelected += OnItemSelected;
                instantiateGameObjectItemSelectBehavior.ItemDeselected += OnItemDeselected;
            }

            newInventoryItem?.item.itemSelectBehavior.SelectItem();
        }

        if (newInventoryItem == null ||
            newInventoryItem.item.statScaleBehavior == null)
        {
            AttackDamage = baseAttackDamage;

            return;
        }

        StatScaleInfo statScaleInfo = newInventoryItem.item.statScaleBehavior.GetStatScaleInfo();

        AttackDamage = baseAttackDamage * statScaleInfo.AttackDamageScaler;
    }

    private void OnItemSelected(GameObject gameObject)
    {
        currentHeldObject = Instantiate(gameObject, transform);
    }

    private void OnItemDeselected()
    {
        Destroy(currentHeldObject);
    }

    protected override void OnDeath()
    {
        Debug.Log("death");
    }

    public bool LoadData(PlayerData data)
    {
        Debug.Log("Pos: " + data.playerPosition);

        transform.position = data.playerPosition;

        return true;
    }

    public void SaveData(Action<IGameData, string> saveCallback, bool gameExit)
    {
        //data.playerPosition = transform.position;

        saveCallback(new PlayerData(transform.position), Path.Combine(ScenePersistentInfo.SceneId, "player"));
    }
}
