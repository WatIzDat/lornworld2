using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private Player player;
    private PlayerInventory playerInventory;

    private bool attackNextUpdate;
    private InventoryItem attackItem;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerInventory = GetComponent<PlayerInventory>();
    }

    private void FixedUpdate()
    {
        if (attackNextUpdate)
        {
            // TODO: switch to new input system, no Input.mousePosition I think
            RaycastHit2D raycastHit = Physics2D.Raycast(
                Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            bool isEntity = false;
            Entity entity = null;

            if (raycastHit.collider != null)
            {
                isEntity = raycastHit.transform.TryGetComponent(out entity);

                if (isEntity)
                {
                    entity.TakeDamage(player.AttackDamage);
                }
            }

            if (attackItem != null && attackItem.item.itemAttackBehavior != null)
            {
                attackItem.item.itemAttackBehavior.Attack(raycastHit, isEntity, entity, Instantiate);
            }

            attackNextUpdate = false;
            attackItem = null;
        }
    }

#pragma warning disable IDE0051, IDE0060
    private void OnAttack(InputValue inputValue)
    {
        Debug.Log("attack");
        attackNextUpdate = true;

        attackItem = playerInventory.SelectedItem;
    }
#pragma warning restore IDE0051, IDE0060
}
