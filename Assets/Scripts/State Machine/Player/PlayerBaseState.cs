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
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movment) * deltaTime);
    }
    protected Vector3 CalculateMovmentDiraction()
    {
        Vector3 movmentDir = Vector3.right * stateMachine.InputManager.MovmentValue.x +
            Vector3.forward * stateMachine.InputManager.MovmentValue.y;
        movmentDir = Quaternion.Euler(0, -45f, 0) * movmentDir;
        return movmentDir;
    }
    protected void FaceMovementDirection(Vector3 movment, float deltaTime)
    {
        Quaternion targetRotation = Quaternion.LookRotation(movment);
        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, targetRotation,
            stateMachine.RotationSpeed * deltaTime);
    }
    protected Vector3 CalculatMovmentDiractionByMouse()
    {
        if (Camera.main == null) return Vector3.zero;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, stateMachine.transform.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            Vector3 direction = targetPoint - stateMachine.transform.position;
            direction.y = 0;

            return direction.normalized;
        }

        return Vector3.zero;
    }
}
