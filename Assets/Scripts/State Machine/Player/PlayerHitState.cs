using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : PlayerBaseState
{
    private readonly int Hit = Animator.StringToHash("Hit");
    private bool alreadyApplyForce;

    public PlayerHitState(PlayerStateMachine stateMachine) : base(stateMachine){}
    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(Hit, 0.1f);
    }
    public override void Tick(float deltaTime)
    {
        TryApplyForce();
        Move(Vector3.zero, deltaTime);
    }

    public override void Exit()
    {

    }

    private void TryApplyForce()
    {
        if (alreadyApplyForce) return;

        stateMachine.ForceReceiver.AddForce(stateMachine.PlayerHealth.LastHitDir 
            * stateMachine.PlayerHealth.HitImpact);

        alreadyApplyForce = true;
    }
}
