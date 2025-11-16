using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Entity, IDataPersistence<GameData>
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

        PlayerInventory.HotbarSelectedIndexChanged += OnHotbarSelectedIndexChanged;

        InventoryUIManager.ArmorChanged += OnArmorChanged;
    }

    private void OnDisable()
    {
        //DataPersistenceManager.LoadTriggered -= LoadData;
        DataPersistenceManager.SaveTriggered -= SaveData;

        PlayerInventory.HotbarSelectedIndexChanged -= OnHotbarSelectedIndexChanged;

        InventoryUIManager.ArmorChanged -= OnArmorChanged;
    }

    private void Start()
    {
        DataPersistenceManager.Instance.LoadObject<GameData>(data => LoadData(data), () => transform.position = Vector2.zero, "player");
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
        transform.position = Vector2.zero;
    }

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

    private void OnHotbarSelectedIndexChanged(int index)
    {
        if (playerInventory.SelectedItem == null ||
            playerInventory.SelectedItem.item.statScaleBehavior == null)
        {
            AttackDamage = baseAttackDamage;

            return;
        }

        StatScaleInfo statScaleInfo = playerInventory.SelectedItem.item.statScaleBehavior.GetStatScaleInfo();

        AttackDamage = baseAttackDamage * statScaleInfo.AttackDamageScaler;
    }

    protected override void OnDeath()
    {
        Debug.Log("death");
    }

    public bool LoadData(GameData data)
    {
        transform.position = data.playerPosition;

        return true;
    }

    public void SaveData(Action<IGameData, string> saveCallback)
    {
        //data.playerPosition = transform.position;

        saveCallback(new GameData(transform.position), "player");
    }
}
