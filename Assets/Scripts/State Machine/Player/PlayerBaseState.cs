using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;
    
    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movment)* deltaTime);
    }
    protected Vector3 CalculateMovment()
    {
        Vector3 movment = Vector3.right * stateMachine.InputManager.MovmentValue.x +
            Vector3.forward * stateMachine.InputManager.MovmentValue.y;
        movment = Quaternion.Euler(0, -45f, 0) * movment;
        return movment;
    }
}
