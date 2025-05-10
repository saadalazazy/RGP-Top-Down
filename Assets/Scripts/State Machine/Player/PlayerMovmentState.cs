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
        stateMachine.Animator.CrossFadeInFixedTime(MovmentBlendtree , 0.1f);
        stateMachine.InputManager.DodgeEvent += OnDodge;
    }
    public override void Tick(float deltaTime)
    {
        Vector3 movment = CalculateMovmentDiraction();
        Move(movment * stateMachine.MovementSpeed, deltaTime);
        if (stateMachine.InputManager.IsAming)
        {
            stateMachine.SwitchStateTo(new PlayerRangedAttackState(stateMachine,
                stateMachine.InputManager.IsMouseUse));
        }
        if (stateMachine.InputManager.IsAttacking)
        {
            stateMachine.Animator.SetFloat(MomvmentSpeed, 0, AnimatorDumpTime, 0.9f);
            stateMachine.SwitchStateTo(new PlayerAttackState(stateMachine, 0));
            return;
        }
        if (stateMachine.InputManager.MovmentValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(MomvmentSpeed, 0, AnimatorDumpTime, deltaTime);
            return;
        }
        stateMachine.Animator.SetFloat(MomvmentSpeed, 1, AnimatorDumpTime, deltaTime);
        FaceMovementDirection(movment, deltaTime);
    }


    public override void Exit()
    {
        stateMachine.InputManager.DodgeEvent -= OnDodge;
    }

    void OnDodge()
    {
        if (stateMachine.InputManager.MovmentValue == Vector2.zero) return;
        stateMachine.SwitchStateTo(new PlayerDodgeState(stateMachine));
    }
}
