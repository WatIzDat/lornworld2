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

    private InventorySlot[] inventorySlots = new InventorySlot[InventorySize];

    private VisualElement root;
    private VisualElement slotContainer;
    private static VisualElement ghostIcon;
    private List<VisualElement> inventoryRows;

    private static bool isDragging;
    private static InventorySlot dragStartSlot;

    [SerializeField]
    private ItemScriptableObject testItem;

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        slotContainer = root.Q<VisualElement>("SlotContainer");
        ghostIcon = root.Q<VisualElement>("GhostIcon");

        ghostIcon.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        ghostIcon.RegisterCallback<PointerUpEvent>(OnPointerUp);

        inventoryRows = slotContainer.Query<VisualElement>("InventoryRow").ToList();
        inventoryRows.Reverse();

        List<InventorySlot> tempSlots = new();

        foreach (VisualElement row in inventoryRows)
        {
            tempSlots.AddRange(row.Query<InventorySlot>().ToList());
        }

        for (int i = 0; i < tempSlots.Count; i++)
        {
            tempSlots[i].index = i;
        }

        inventorySlots = tempSlots.ToArray();

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
        AddItem(testItem, 1);
        AddItem(testItem, 1);
        AddItem(testItem, 3);
        AddItem(testItem, 2);

        items[10] = new InventoryItem(testItem);
        items[12] = new InventoryItem(testItem);
    }

    private void OnEnable()
    {
        items.CollectionChanged += OnItemsChanged;
    }

    private void OnDisable()
    {
        items.CollectionChanged -= OnItemsChanged;
    }

    public static void StartDrag(Vector2 position, InventorySlot startSlot)
    {
        isDragging = true;
        dragStartSlot = startSlot;

        ghostIcon.style.top = position.y - (ghostIcon.layout.height / 2);
        ghostIcon.style.left = position.x - (ghostIcon.layout.width / 2);

        ghostIcon.style.backgroundImage = startSlot.inventoryItem.Item.sprite.texture;

        ghostIcon.style.visibility = Visibility.Visible;
    }

    private void OnPointerMove(PointerMoveEvent evt)
    {
        if (!isDragging)
        {
            return;
        }

        ghostIcon.style.top = evt.position.y - (ghostIcon.layout.height / 2);
        ghostIcon.style.left = evt.position.x - (ghostIcon.layout.width / 2);
    }

    private void OnPointerUp(PointerUpEvent evt)
    {
        if (!isDragging)
        {
            return;
        }

        IEnumerable<InventorySlot> slots = inventorySlots.Where(slot =>
               slot.worldBound.Overlaps(ghostIcon.worldBound));

        InventorySlot closestSlot = null;

        if (slots.Count() != 0)
        {
            closestSlot = slots.OrderBy(slot => Vector2.Distance
                (slot.worldBound.position, ghostIcon.worldBound.position)).First();
        }

        if (closestSlot != null &&
            closestSlot != dragStartSlot &&
            CanItemBeSet(closestSlot.index, dragStartSlot.inventoryItem))
        {
            items[closestSlot.index] = dragStartSlot.inventoryItem;

            items[dragStartSlot.index] = null;
        }
        else
        {
            dragStartSlot.icon.image =
                  dragStartSlot.inventoryItem.Item.sprite.texture;
        }

        isDragging = false;
        dragStartSlot = null;
        ghostIcon.style.visibility = Visibility.Hidden;
    }

    private void OnItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        //InventorySlot slot = inventoryRows[e.NewStartingIndex / InventoryWidth]
        //    .Query<InventorySlot>()
        //    .AtIndex(e.NewStartingIndex % InventoryWidth);

        InventorySlot slot = inventorySlots[e.NewStartingIndex];

        InventoryItem newInventoryItem = (InventoryItem)e.NewItems[0];
        InventoryItem oldInventoryItem = (InventoryItem)e.OldItems[0];

        if (oldInventoryItem == null)
        {
            slot.SetItem(newInventoryItem);

            return;
        }

        if (newInventoryItem == null)
        {
            slot.DropItem();

            return;
        }

        if (newInventoryItem.Item == oldInventoryItem.Item &&
            CanItemBeSet(e.NewStartingIndex, oldInventoryItem))
        {
            slot.SetItem(new InventoryItem(oldInventoryItem.Item, oldInventoryItem.StackSize + newInventoryItem.StackSize));

            return;
        }

        //slot.item = inventoryItem.Item;
        //slot.icon.sprite = inventoryItem.Item.sprite;
        //slot.stackSizeLabel.text = inventoryItem.StackSize.ToString();

        slot.SetItem(newInventoryItem);
    }

    private bool CanItemBeSet(int itemIndex, InventoryItem item)
    {
        if (items[itemIndex] == null)
        {
            return true;
        }

        bool isSameItem = items[itemIndex].Item == item.Item;
        bool isNotOverflowing = items[itemIndex].StackSize + item.StackSize <= items[itemIndex].Item.maxStackSize;

        return isSameItem && isNotOverflowing;
    }


    private bool AddItem(ItemScriptableObject item, int stack)
    {
        if (stack > item.maxStackSize)
        {
            return false;
        }

        bool hasEmptySlot = false;
        int emptyIndex = -1;

        int remainingStack = stack;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null && hasEmptySlot == false)
            {
                emptyIndex = i;
                hasEmptySlot = true;
            }
            else if (
                items[i] != null &&
                items[i].Item == item && 
                items[i].StackSize < item.maxStackSize)
            {
                int usedStack = item.maxStackSize - items[i].StackSize;

                if (usedStack > stack)
                {
                    usedStack = stack;
                }

                remainingStack -= usedStack;

                items[i] = new InventoryItem(item, items[i].StackSize + usedStack);

                Debug.Log("test");
            }
        }

        if (hasEmptySlot && remainingStack >= 1)
        {
            items[emptyIndex] = new InventoryItem(item, remainingStack);

            return true;
        }

        return false;
    }
}
