using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChomperStateManager : MonoBehaviour
{
    ChomperBaseState currentState;

    public ChomperIdleState idle = new ChomperIdleState();
    public ChomperWalkingState walk = new ChomperWalkingState();

    private float moveSpeed = 2f;
    private float currentSpeed;

    private void Start()
    {
        currentState = idle;
        currentSpeed = moveSpeed;
        currentState.EnterState(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            KillZone.GameOver.Invoke();
        }
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(ChomperBaseState chomper)
    {
        currentState = chomper;
        currentState.EnterState(this);
    }

    public void RestoreSpeed() => currentSpeed = moveSpeed;

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public void SetCurrentSpeed(float speed)
    {
        currentSpeed = speed;
    }
}
