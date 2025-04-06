using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBaseState
{
    public string stateName;
    public UnitStateMachine stateMachine;
    public UnitBaseState(string stateName, UnitStateMachine unitStateMachine)
    {
        stateMachine = unitStateMachine;
        this.stateName = stateName;
    }
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Tick() { }
    public virtual void PhysicsTick() { }
    public virtual void PostTick() { }

}
