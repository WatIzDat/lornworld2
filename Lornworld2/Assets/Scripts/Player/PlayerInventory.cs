using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private InventoryUIManager inventoryUIManager;

#pragma warning disable IDE0051
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
#pragma warning restore IDE0051
}
