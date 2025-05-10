using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangedAttackState : PlayerBaseState
{
    private bool mouseUse;
    private readonly int AnimationAimName = Animator.StringToHash("Aim");
    private readonly int AnimationShootName = Animator.StringToHash("Shoot");
    public PlayerRangedAttackState(PlayerStateMachine stateMachine, bool mouseUse) : base(stateMachine)
    {
        this.mouseUse = mouseUse;
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(AnimationAimName, 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movmentDir;
        Vector3 lastMovementDir;
        if (mouseUse)
        {
            movmentDir = CalculatMovmentDiractionByMouse();
        }
        else
        {
            movmentDir = CalculateMovmentDiraction();
        }
        if (!stateMachine.InputManager.IsAming)
        {
            stateMachine.SwitchStateTo(new PlayerMovmentState(stateMachine));
        }
        if(stateMachine.InputManager.IsAttacking && stateMachine.ArrowFireHandler.canShoot)
        {
            stateMachine.Animator.CrossFadeInFixedTime(AnimationShootName, 0.1f);
            stateMachine.ForceReceiver.AddForce(-stateMachine.transform.forward * stateMachine.ArrowFireHandler.impact);
        }
        if (stateMachine.InputManager.MovmentValue == Vector2.zero && !mouseUse) return;
        lastMovementDir = movmentDir;
        Move(Vector3.zero, deltaTime);
        FaceMovementDirection(lastMovementDir, deltaTime);
    }

    public override void Exit()
    {

    }

}