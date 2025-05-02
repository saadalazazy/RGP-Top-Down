using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerMovmentState : PlayerBaseState
{
    private readonly int isMoving = Animator.StringToHash("isMoving");
    private readonly int TargetingBlendtree = Animator.StringToHash("TargetingBlendtree");

    private const float AnimatorDumpTime = 0.1f;
    public PlayerMovmentState(PlayerStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter()
    {
        stateMachine.InputManager.TargetEvent += OnTriger;
        Debug.Log("movment state");
    }
    public override void Tick(float deltaTime)
    {
        Vector3 movment = CalculateMovment();
        Move(movment * stateMachine.MovementSpeed , deltaTime);
        if (stateMachine.InputManager.MovmentValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(isMoving, 0, AnimatorDumpTime, deltaTime);
            return;
        }
        stateMachine.Animator.SetFloat(isMoving, 1, AnimatorDumpTime, deltaTime);
        FaceMovementDirection(movment , deltaTime);
    }
    public override void Exit()
    {
        stateMachine.InputManager.TargetEvent -= OnTriger;
        Debug.Log("cancel movment state");
    }
    void OnTriger()
    {
        if (!stateMachine.Targeter.SelectTarget()) return;
        stateMachine.Animator.Play(TargetingBlendtree);
        stateMachine.SwitchStateTo(new PlayerTargetState(stateMachine));
    }
    private Vector3 CalculateMovment()
    {
        Vector3 movment = Vector3.right * stateMachine.InputManager.MovmentValue.x +
            Vector3.forward * stateMachine.InputManager.MovmentValue.y;
        movment = Quaternion.Euler(0, -45f, 0) * movment;
        return movment;
    }
    private void FaceMovementDirection(Vector3 movment , float deltaTime)
    {
        Quaternion targetRotation = Quaternion.LookRotation(movment);
        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, targetRotation,
            stateMachine.RotationSpeed * deltaTime);
    }
}
