using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUIManager : MonoBehaviour
{
    public const int InventoryWidth = 9;
    public const int InventoryHeight = 4; // including hotbar

    private const int InventorySize = InventoryWidth * InventoryHeight;

    // bottom left to top right
    // TODO: maybe separate items outside of view logic
    private readonly ObservableCollection<InventoryItem> items = new();

    public InventoryItem[] HotbarItems => items.ToArray()[..InventoryWidth];

    private InventorySlot[] inventorySlots = new InventorySlot[InventorySize];

    private VisualElement root;
    private VisualElement panel;
    private VisualElement slotContainer;
    private VisualElement ghostIcon;
    private List<VisualElement> inventoryRows;

    private bool isDragging;
    private InventorySlot dragStartSlot;
    private InventoryItem draggedItem;
    private int draggedStackSize;

    public bool IsInventoryOpen { get; private set; }

    public static event Action<int, InventoryItem> InventoryChanged;

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        panel = root.Q<VisualElement>("Panel");
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
            tempSlots[i].inventoryUIManager = this;
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
        AddItem(ItemRegistry.Instance.GetEntry(ItemIds.GrassItem), 40);
        AddItem(ItemRegistry.Instance.GetEntry(ItemIds.StoneItem), 17);

        //AddItem(testItem, 1);
        //AddItem(testItem, 1);
        //AddItem(testItem, 3);
        //AddItem(testItem, 2);

        //items[10] = new InventoryItem(testItem);
        //items[12] = new InventoryItem(testItem);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            TryRemoveItem(ItemRegistry.Instance.GetEntry(ItemIds.GrassItem), 27);
        }
    }

    private void OnEnable()
    {
        items.CollectionChanged += OnItemsChanged;
    }

    private void OnDisable()
    {
        items.CollectionChanged -= OnItemsChanged;
    }

    public void OpenInventory()
    {
        panel.style.display = DisplayStyle.Flex;

        IsInventoryOpen = true;
    }

    public void CloseInventory()
    {
        panel.style.display = DisplayStyle.None;

        IsInventoryOpen = false;
    }

    public void StartDrag(Vector2 position, InventorySlot startSlot, int stackSize)
    {
        stackSize = Mathf.Clamp(stackSize, 1, startSlot.InventoryItem.item.maxStackSize);

        //startSlot.SetItem(
        //    new InventoryItem(
        //        startSlot.InventoryItem.Item,
        //        startSlot.InventoryItem.StackSize - stackSize));


        //startSlot.Icon.image = null;

        isDragging = true;
        dragStartSlot = startSlot;
        draggedItem = startSlot.InventoryItem;
        draggedStackSize = stackSize;

        ghostIcon.style.top = position.y - (ghostIcon.layout.height / 2);
        ghostIcon.style.left = position.x - (ghostIcon.layout.width / 2);

        ghostIcon.style.backgroundImage = startSlot.InventoryItem.item.sprite.texture;

        ghostIcon.style.visibility = Visibility.Visible;

        items[startSlot.index] = new InventoryItem(
            startSlot.InventoryItem.item,
            startSlot.InventoryItem.stackSize - stackSize);
    }

    public bool ContainsItem(ItemScriptableObject targetItem, int targetStack)
    {
        int stack = 0;

        foreach (InventoryItem item in items)
        {
            if (item?.item == targetItem)
            {
                stack += item.stackSize;

                if (stack >= targetStack)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void RemoveItem(ItemScriptableObject targetItem, int targetStack)
    {
        int stack = targetStack;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i]?.item == targetItem)
            {
                if (stack < items[i].stackSize)
                {
                    items[i] = items[i].AddStack(-stack);

                    return;
                }
                else
                {
                    stack -= items[i].stackSize;

                    items[i] = null;

                    if (stack == 0)
                    {
                        return;
                    }
                }
            }
        }

        return;
    }

    public bool TryRemoveItem(ItemScriptableObject targetItem, int targetStack)
    {
        if (!ContainsItem(targetItem, targetStack))
        {
            return false;
        }

        RemoveItem(targetItem, targetStack);

        return true;
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
        
        InventoryItem newInventoryItem = draggedItem.WithStack(draggedStackSize);

        if (closestSlot != null &&
            closestSlot != dragStartSlot &&
            CanItemBeSet(closestSlot.index, newInventoryItem))
        {
            if (items[closestSlot.index] == null)
            {
                items[closestSlot.index] = newInventoryItem;
            }
            else
            {
                items[closestSlot.index] = items[closestSlot.index].AddStack(draggedStackSize);
            }

            if (dragStartSlot.InventoryItem.stackSize <= 0)
            {
                items[dragStartSlot.index] = null;
            }
            //else
            //{
            //    items[dragStartSlot.index] = draggedItem;
            //}
        }
        else
        {
            //dragStartSlot.inventoryItem = dragStartSlot.inventoryItem.AddStack(draggedStackSize);

            //dragStartSlot.SetItem(dragStartSlot.InventoryItem.AddStack(draggedStackSize));

            items[dragStartSlot.index] = draggedItem;

            //dragStartSlot.icon.image =
            //      dragStartSlot.inventoryItem.Item.sprite.texture;
        }

        isDragging = false;
        dragStartSlot = null;
        ghostIcon.style.visibility = Visibility.Hidden;
        draggedStackSize = 0;
        draggedItem = null;
    }

    private void OnItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        //InventorySlot slot = inventoryRows[e.NewStartingIndex / InventoryWidth]
        //    .Query<InventorySlot>()
        //    .AtIndex(e.NewStartingIndex % InventoryWidth);


        InventorySlot slot = inventorySlots[e.NewStartingIndex];

        InventoryItem newInventoryItem = (InventoryItem)e.NewItems[0];

        InventoryChanged?.Invoke(e.NewStartingIndex, newInventoryItem);

        //InventoryItem oldInventoryItem = (InventoryItem)e.OldItems[0];

        //if (oldInventoryItem == null)
        //{
        //    slot.SetItem(newInventoryItem);

        //    return;
        //}

        //if (newInventoryItem.Item == oldInventoryItem.Item &&
        //    CanItemBeSet(e.NewStartingIndex, oldInventoryItem))
        //{
        //    slot.SetItem(new InventoryItem(oldInventoryItem.Item, oldInventoryItem.StackSize + newInventoryItem.StackSize));

        //    return;
        //}

        if (newInventoryItem == null)
        {
            slot.DropItem();

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

        bool isSameItem = items[itemIndex].item == item.item;
        bool isNotOverflowing = items[itemIndex].stackSize + item.stackSize <= items[itemIndex].item.maxStackSize;

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
                items[i].item == item && 
                items[i].stackSize < item.maxStackSize)
            {
                int usedStack = item.maxStackSize - items[i].stackSize;

                if (usedStack > stack)
                {
                    usedStack = stack;
                }

                remainingStack -= usedStack;

                items[i] = new InventoryItem(item, items[i].stackSize + usedStack);

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
