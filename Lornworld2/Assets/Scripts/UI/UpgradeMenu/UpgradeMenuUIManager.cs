using UnityEngine;

// Temporary debug code for now
public class UpgradeMenuUIManager : MonoBehaviour
{
    [SerializeField]
    private PlayerInventory playerInventory;

    [SerializeField]
    private CoalUpgradedWeaponItemAttackBehavior coalUpgradedWeaponItemAttackBehavior;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (playerInventory.SelectedItem.item.isWeapon && playerInventory.SelectedItem.item.makeUniqueRuntimeInstances)
            {
                playerInventory.SelectedItem.item.SetItemAttackBehavior(coalUpgradedWeaponItemAttackBehavior);
            }
        }
    }
}
