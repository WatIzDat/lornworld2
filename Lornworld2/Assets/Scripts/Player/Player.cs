using UnityEngine;

public class Player : Entity
{
    [SerializeField]
    private float baseAttackDamage;

    public float AttackDamage { get; private set; }

    [SerializeField]
    private int hunger;

    private PlayerInventory playerInventory;

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

    private void OnHotbarSelectedIndexChanged(int index)
    {
        if (playerInventory.SelectedItem == null ||
            playerInventory.SelectedItem.Item.statScaleBehavior == null)
        {
            AttackDamage = baseAttackDamage;

            return;
        }

        StatScaleInfo statScaleInfo = playerInventory.SelectedItem.Item.statScaleBehavior.GetStatScaleInfo();

        AttackDamage = baseAttackDamage * statScaleInfo.AttackDamageScaler;
    }

    protected override void OnDeath()
    {
        Debug.Log("death");
    }
}
