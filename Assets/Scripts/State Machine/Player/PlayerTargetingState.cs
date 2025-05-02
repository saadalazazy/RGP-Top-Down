using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetState : PlayerBaseState
{
    public PlayerTargetState(PlayerStateMachine stateMachine) : base(stateMachine){}

    private readonly int MovmentBlendtree = Animator.StringToHash("MovmentBlendtree");
    public override void Enter()
    {
        stateMachine.InputManager.CancelEvent += OnCancel;
        Debug.Log("target state");
    }
    public override void Tick(float deltaTime)
    {
        if(stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchStateTo(new PlayerMovmentState(stateMachine));
            return;
        }
        Move(stateMachine.ForceReceiver.Movment , deltaTime);
    }

    public override void Exit()
    {
        stateMachine.InputManager.CancelEvent -= OnCancel;
        Debug.Log("cancel target state");
    }
    private void OnCancel()
    {
        stateMachine.Targeter.ClearCurrentTarget();
        stateMachine.Animator.Play(MovmentBlendtree);
        stateMachine.SwitchStateTo(new PlayerMovmentState(stateMachine));
    }
}
