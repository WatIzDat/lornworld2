using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private Player player;

    private bool attackNextUpdate;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        if (attackNextUpdate)
        {
            // TODO: switch to new input system, no Input.mousePosition I think
            RaycastHit2D raycastHit = Physics2D.Raycast(
                Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (raycastHit.collider != null)
            {
                bool isEntity = raycastHit.transform.TryGetComponent(out Entity entity);

                if (isEntity)
                {
                    entity.TakeDamage(player.AttackDamage);
                }
            }

            attackNextUpdate = false;
        }
    }

#pragma warning disable IDE0051, IDE0060
    private void OnAttack(InputValue inputValue)
    {
        Debug.Log("attack");
        attackNextUpdate = true;
    }
#pragma warning restore IDE0051, IDE0060
}
