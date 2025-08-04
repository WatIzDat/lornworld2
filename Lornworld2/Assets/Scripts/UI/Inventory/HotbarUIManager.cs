using UnityEngine;
using UnityEngine.UIElements;

public class HotbarUIManager : MonoBehaviour
{
    private InventorySlot[] hotbarSlots = new InventorySlot[InventoryUIManager.InventoryWidth];

    private VisualElement root;
    private VisualElement slotContainer;

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        slotContainer = root.Q<VisualElement>("SlotContainer");

        hotbarSlots = slotContainer.Query<InventorySlot>().ToList().ToArray();

        foreach (InventorySlot slot in hotbarSlots)
        {
            slot.isHotbarSlot = true;
        }
    }

    private void OnEnable()
    {
        InventoryUIManager.InventoryChanged += OnInventoryChanged;
    }

    private void OnDisable()
    {
        InventoryUIManager.InventoryChanged -= OnInventoryChanged;
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
