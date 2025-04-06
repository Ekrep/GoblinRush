using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStateMachine : MonoBehaviour
{
    public UnitBaseState currentState;
    private void Start()
    {
        if (currentState != null)
        {
            currentState.Enter();
        }
    }
    private void Update()
    {
        if (currentState != null)
        {
            currentState.Tick();
        }
    }
    private void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.PhysicsTick();
        }
    }
    private void LateUpdate()
    {
        if (currentState != null)
        {
            currentState.PostTick();
        }
    }
    public void ChangeState(UnitBaseState state)
    {
        currentState.Exit();
        currentState = state;
        currentState.Enter();
    }


}
