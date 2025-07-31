using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUIManager : MonoBehaviour
{
    private const int InventoryWidth = 9;
    private const int InventoryHeight = 4; // including hotbar

    private const int InventorySize = InventoryWidth * InventoryHeight;

    // bottom left to top right
    private readonly ObservableCollection<InventoryItem> items = new();

    private VisualElement root;
    private VisualElement slotContainer;
    private List<VisualElement> inventoryRows;

    [SerializeField]
    private ItemScriptableObject testItem;

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        slotContainer = root.Q<VisualElement>("SlotContainer");

        inventoryRows = slotContainer.Query<VisualElement>("InventoryRow").ToList();
        inventoryRows.Reverse();

        for (int i = 0; i < InventorySize; i++)
        {
            items.Add(null);
        }

        //for (int i = 0; i < InventorySize; i++)
        //{
        //    InventorySlot slot = new();

        //    slotContainer.Add(slot);
        //}
    }

    private void Start()
    {
        AddItem(testItem);
    }

    private void OnEnable()
    {
        items.CollectionChanged += OnItemsChanged;
    }

    private void OnDisable()
    {
        items.CollectionChanged -= OnItemsChanged;
    }

    private void OnItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        InventorySlot slot = inventoryRows[e.NewStartingIndex / InventoryWidth]
            .Query<InventorySlot>()
            .AtIndex(e.NewStartingIndex % InventoryWidth);

        InventoryItem item = (InventoryItem)e.NewItems[0];

        slot.icon.sprite = item.item.sprite;
    }

    private bool AddItem(ItemScriptableObject item)
    {
        InventoryItem inventoryItem = new(item);

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                items[i] = inventoryItem;

                return true;
            }
        }

        return false;
    }
}
