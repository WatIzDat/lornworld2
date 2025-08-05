using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private InventoryUIManager inventoryUIManager;

    private int selectedIndex;
    private InventoryItem SelectedItem => inventoryUIManager.HotbarItems[selectedIndex];

    public static event Action<int> HotbarSelectedIndexChanged;

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
            selectedIndex < inventoryUIManager.HotbarItems.Length - 1)
        {
            selectedIndex++;

            HotbarSelectedIndexChanged?.Invoke(selectedIndex);
        }
        else if (direction < 0 && selectedIndex > 0)
        {
            selectedIndex--;

            HotbarSelectedIndexChanged?.Invoke(selectedIndex);
        }

        Debug.Log(selectedIndex + " " + SelectedItem);
    }

    private void OnUseItem(InputValue inputValue)
    {
        SelectedItem?.UseItem();
    }
#pragma warning restore IDE0051, IDE0060
}
