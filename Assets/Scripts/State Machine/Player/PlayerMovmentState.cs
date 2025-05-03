using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerMovmentState : PlayerBaseState
{
    private readonly int MomvmentSpeed = Animator.StringToHash("MomvmentSpeed");
    private readonly int MovmentBlendtree = Animator.StringToHash("MovmentBlendtree");

    private const float AnimatorDumpTime = 0.1f;
    public PlayerMovmentState(PlayerStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter()
    {
        stateMachine.Animator.Play(MovmentBlendtree);
    }
    public override void Tick(float deltaTime)
    {
        Vector3 movment = CalculateMovment();
        Move(movment * stateMachine.MovementSpeed , deltaTime);
        if (stateMachine.InputManager.MovmentValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(MomvmentSpeed, 0, AnimatorDumpTime, deltaTime);
            return;
        }
        stateMachine.Animator.SetFloat(MomvmentSpeed, 1, AnimatorDumpTime, deltaTime);
        FaceMovementDirection(movment , deltaTime);
    }
    public override void Exit()
    {

    }
    private void FaceMovementDirection(Vector3 movment , float deltaTime)
    {
        Quaternion targetRotation = Quaternion.LookRotation(movment);
        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, targetRotation,
            stateMachine.RotationSpeed * deltaTime);
    }
}
