using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    private readonly int Dead = Animator.StringToHash("Dead");
    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(Dead, 0.1f);
    }
    public override void Tick(float deltaTime)
    {

    }

    public override void Exit()
    {

    }

}
