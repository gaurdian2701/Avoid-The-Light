using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;

    private Rigidbody2D rb;
    private CapsuleCollider2D playerCollider;
    private float moveDirection;
    private Color rayColor;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        moveDirection = 0f;
        rayColor = Color.white;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
    }
    public void MovePlayer(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        moveDirection = context.ReadValue<float>();


        Vector3 newRotation;

        if (moveDirection < 0f)
        {
            newRotation = new Vector3(0, 180, 0);
            transform.eulerAngles = newRotation;
        }

        else if (moveDirection > 0f)
        {
            newRotation = new Vector3(0, 0, 0);
            transform.eulerAngles = newRotation;
        }
    }

    public void PlayerJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        rb.velocity = new Vector2(rb.velocity.x, jumpPower);
    }

    private bool IsGrounded()
    {
        if (Physics2D.Raycast(playerCollider.bounds.center, -transform.up, 1.3f, LayerMask.GetMask("Ground")))
        {
            rayColor = Color.green;
            return true;
        }

        rayColor = Color.red;
        return false;
    }
}
