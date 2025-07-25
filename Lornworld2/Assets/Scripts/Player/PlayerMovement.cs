using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float moveSpeed = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

#pragma warning disable IDE0051
    private void OnMove(InputValue inputValue)
    {
        Vector2 moveDir = inputValue.Get<Vector2>();

        rb.linearVelocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);

        if (moveDir.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveDir.x < 0)
        {
            spriteRenderer.flipX = false;
        }
    }
#pragma warning restore IDE0051
}
