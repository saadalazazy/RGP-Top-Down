using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{
    public PlayerDodgeState(PlayerStateMachine stateMachine) : base(stateMachine){}
    Vector3 movment;
    float remaningTime;
    private readonly int AnimationDodgeName = Animator.StringToHash("Dodge");
    public override void Enter()
    {
        movment = CalculateMovmentDiraction();
        remaningTime = stateMachine.Dodge.Time;
        stateMachine.Animator.CrossFadeInFixedTime(AnimationDodgeName, 0.1f);
        stateMachine.Effects.PlayPlayerDashVfx();
    }
    public override void Tick(float deltaTime)
    {
        Move(movment * stateMachine.Dodge.Speed, deltaTime);
        remaningTime -= deltaTime;
        if(remaningTime <= 0)
        {
            stateMachine.SwitchStateTo(new PlayerMovmentState(stateMachine));
        }
        FaceMovementDirection(movment, deltaTime);
    }

    public override void Exit()
    {

    }

}
