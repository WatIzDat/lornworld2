using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUIManager : MonoBehaviour
{
    private const int InventoryWidth = 9;
    private const int InventoryHeight = 3;

    private const int InventorySize = InventoryWidth * InventoryHeight;

    private InventoryItem[] items = new InventoryItem[InventorySize];

    private VisualElement root;
    private VisualElement slotContainer;

    [SerializeField]
    private ItemScriptableObject testItem;

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        slotContainer = root.Q<VisualElement>("SlotContainer");

        //for (int i = 0; i < InventorySize; i++)
        //{
        //    InventorySlot slot = new();

        //    slotContainer.Add(slot);
        //}
    }
}
