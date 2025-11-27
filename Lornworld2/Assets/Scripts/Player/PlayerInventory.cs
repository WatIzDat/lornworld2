using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private InventoryUIManager inventoryUIManager;

    [SerializeField]
    private CraftingMenuUIManager craftingMenuUIManager;

    private int prevSelectedIndex;

    private int selectedIndex;
    private int SelectedIndex
    {
        get => selectedIndex;
        set
        {
            prevSelectedIndex = selectedIndex;
            selectedIndex = value;
        }
    }

    public InventoryItem PrevSelectedItem => inventoryUIManager.HotbarItems[prevSelectedIndex];
    public InventoryItem SelectedItem => inventoryUIManager.HotbarItems[SelectedIndex];

    public static event Action<int> HotbarSelectedIndexChanged;

    private bool dropItemStackModifierPressed;
    private bool pickUpItemNextUpdate;

    private void Start()
    {
        HotbarSelectedIndexChanged?.Invoke(0);
    }

    private void FixedUpdate()
    {
        if (pickUpItemNextUpdate)
        {
            Collider2D itemCollider = Physics2D.OverlapCircleAll(transform.position, 1f)
                .Where(c => c.TryGetComponent(out DroppedItem _))
                .OrderBy(c => Vector2.Distance(transform.position, c.transform.position))
                .FirstOrDefault();

            if (itemCollider != null)
            {
                DroppedItem item = itemCollider.GetComponent<DroppedItem>();

                inventoryUIManager.AddItem(item.Item, item.StackSize);

                Destroy(item.gameObject);
            }

            pickUpItemNextUpdate = false;
        }
    }

    public void RemoveFromSelectedItem()
    {
        inventoryUIManager.AddStackToItem(SelectedIndex, -1);
    }

#pragma warning disable IDE0051, IDE0060
    private void OnToggleInventory(InputValue inputValue)
    {
        if (inventoryUIManager.IsInventoryOpen)
        { 
            inventoryUIManager.CloseInventory();
        }
        else
        {
            inventoryUIManager.OpenInventory();
        }
    }

    private void OnToggleCraftingMenu(InputValue inputValue)
    {
        if (craftingMenuUIManager.IsCraftingMenuOpen)
        {
            craftingMenuUIManager.CloseCraftingMenu();
        }
        else
        {
            craftingMenuUIManager.OpenCraftingMenu();
        }
    }

    private void OnNavigateHotbar(InputValue inputValue)
    {
        float direction = inputValue.Get<float>();

        // ignore tiny values
        if (direction < 0.01f &&
            direction > -0.01f)
        {
            return;
        }

        if (direction > 0 &&
            SelectedIndex < inventoryUIManager.HotbarItems.Length - 1)
        {
            SelectedIndex++;

            HotbarSelectedIndexChanged?.Invoke(SelectedIndex);
        }
        else if (direction < 0 && SelectedIndex > 0)
        {
            SelectedIndex--;

            HotbarSelectedIndexChanged?.Invoke(SelectedIndex);
        }

        //Debug.Log(selectedIndex + " " + SelectedItem);
    }

    private void OnUseItem(InputValue inputValue)
    {
        SelectedItem?.UseItem();
    }

    private void OnDropItem(InputValue inputValue)
    {
        if (!inventoryUIManager.IsInventoryOpen || inventoryUIManager.hoveredSlot == null)
        {
            return;
        }

        inventoryUIManager.DropHoveredItem(transform.position, dropItemStackModifierPressed);
    }

    private void OnDropItemStackModifier(InputValue inputValue)
    {
        dropItemStackModifierPressed = inputValue.isPressed;
    }

    private void OnPickUpItem(InputValue inputValue)
    {
        pickUpItemNextUpdate = true;
    }
#pragma warning restore IDE0051, IDE0060
}
