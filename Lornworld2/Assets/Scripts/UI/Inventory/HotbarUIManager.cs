using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

public class HotbarUIManager : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private InventorySlot[] hotbarSlots = new InventorySlot[InventoryUIManager.InventoryWidth];

    private InventorySlot selectedHotbarSlot;

    private VisualElement root;
    private VisualElement slotContainer;
    private VisualElement statsContainer;

    private Label healthLabel;

    [CreateProperty]
    private string FormattedHealth => $"{player.Health}/{player.MaxHealth}";

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        slotContainer = root.Q<VisualElement>("SlotContainer");
        statsContainer = root.Q<VisualElement>("StatsContainer");

        hotbarSlots = slotContainer.Query<InventorySlot>().ToList().ToArray();

        healthLabel = statsContainer.Q<Label>("Health");

        DataBinding healthBinding = new()
        {
            bindingMode = BindingMode.ToTarget,
            dataSource = this,
            dataSourcePath = new PropertyPath(nameof(FormattedHealth))
        };

        healthLabel.SetBinding("text", healthBinding);

        selectedHotbarSlot = hotbarSlots[0];

        foreach (InventorySlot slot in hotbarSlots)
        {
            slot.isHotbarSlot = true;
        }
    }

    private void OnEnable()
    {
        InventoryUIManager.InventoryChanged += OnInventoryChanged;

        PlayerInventory.HotbarSelectedItemChanged += OnHotbarSelectedItemChanged;
    }

    private void OnDisable()
    {
        InventoryUIManager.InventoryChanged -= OnInventoryChanged;

        PlayerInventory.HotbarSelectedItemChanged -= OnHotbarSelectedItemChanged;
    }

    private void OnHotbarSelectedItemChanged(int newIndex, InventoryItem oldInventoryItem, InventoryItem newInventoryItem)
    {
        selectedHotbarSlot.RemoveFromClassList("hotbar-selected-slot");

        selectedHotbarSlot = hotbarSlots[newIndex];

        selectedHotbarSlot.AddToClassList("hotbar-selected-slot");
    }

    private void OnInventoryChanged(int index, InventoryItem item)
    {
        if (index < hotbarSlots.Length)
        { 
            if (item == null)
            {
                hotbarSlots[index].DropItem();
            }
            else
            {
                hotbarSlots[index].SetItem(item);
            }
        }
    }
}
