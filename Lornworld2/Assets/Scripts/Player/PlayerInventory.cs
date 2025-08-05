using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private InventoryUIManager inventoryUIManager;

    private int selectedIndex;
    private InventoryItem SelectedItem => inventoryUIManager.HotbarItems[selectedIndex];

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
        }
        else if (direction < 0 && selectedIndex > 0)
        {
            selectedIndex--;
        }

        Debug.Log(selectedIndex + " " + SelectedItem);
    }
#pragma warning restore IDE0051, IDE0060
}
