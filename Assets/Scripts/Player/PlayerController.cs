using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float boostPower;
    [SerializeField] private float gravityRate;

    private Rigidbody2D rb;
    private CapsuleCollider2D playerCollider;
    private ParticleController particleController;

    private float moveDirection;
    private float currentBoostPower;
    private float downwardVelocity;

    private bool canBoost;

    public static Action FillBar;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        particleController = GetComponent<ParticleController>();

        moveDirection = 0f;
        currentBoostPower = 0f;
        downwardVelocity = 0f;

        canBoost = true;

        BoostBar.BoostEnabled += SetCanBoost;
    }

    private void Start()
    {
        CheckIfGrounded();
    }

    private void FixedUpdate()
    {
        ExecuteMovement();
    }

    private void OnDestroy()
    {
        BoostBar.BoostEnabled -= SetCanBoost;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckIfGrounded();
    }

    private void ExecuteMovement()
    {
        rb.velocity = new Vector2(moveDirection * moveSpeed, currentBoostPower - IncreaseDownwardVelocity());
    }

    private void CheckIfGrounded()
    {
        if (IsGrounded())
            downwardVelocity = 0f;

        else
            downwardVelocity = rb.gravityScale;
    }

    private float IncreaseDownwardVelocity()
    {
        if (rb.velocity.y >= 0f)
            return 1f;

        return downwardVelocity += gravityRate;
    }

    private void SetCanBoost(bool boostStatus)
    {
        canBoost = boostStatus;

        if (!boostStatus)
            currentBoostPower = 0f;
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
        if (context.performed)
        {
            downwardVelocity = 1f;
            currentBoostPower = canBoost ? boostPower : 0f;
            FillBar.Invoke();
            particleController.EnableParticles();
        }
        
        if(context.canceled)
        {
            currentBoostPower = 0f;
            CheckIfGrounded();
            FillBar.Invoke();
            particleController.DisableParticles();
        }
    }

    private bool IsGrounded()
    {
        if (Physics2D.Raycast(playerCollider.bounds.center, -transform.up, 0.9f, LayerMask.GetMask("Ground")))
            return true;

        return false;
    }
}
