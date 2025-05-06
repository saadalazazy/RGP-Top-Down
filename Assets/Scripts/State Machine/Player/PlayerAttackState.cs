using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private Attack attack;
    private float previousFrameTime;
    private bool alreadyApplyForce;
    public PlayerAttackState(PlayerStateMachine stateMachine , int attackIndex) : base(stateMachine)
    {
        attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
    }
    public override void Tick(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
        float normalizeTime = GetNormalizeTime();
        if (normalizeTime >= previousFrameTime && normalizeTime < 1) 
        {
            if (normalizeTime >= attack.ForceTime)
            {
                TryApplyForce();
            }
            if (stateMachine.InputManager.IsAttacking)
            {
                TryComboAttack(normalizeTime);
            }
        }
        else
        {
            if(normalizeTime > attack.ComboAttackTime)
            {
                if(stateMachine.InputManager.IsAttacking)
                {
                    stateMachine.SwitchStateTo(new PlayerAttackState(stateMachine, 0));
                }
                else
                {
                    stateMachine.SwitchStateTo(new PlayerMovmentState(stateMachine));
                }
            }
        }
        

    }
    public override void Exit()
    {

    }
    private void TryApplyForce()
    {
        if (alreadyApplyForce) return;

        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attack.Force);

        alreadyApplyForce = true;
    }
    private void TryComboAttack(float normalizeTime)
    {
        if (attack.ComboStateIndex == -1) return;
        if (normalizeTime < attack.ComboAttackTime) return;

        stateMachine.SwitchStateTo(new PlayerAttackState(stateMachine, attack.ComboStateIndex));

    }

    private float GetNormalizeTime()
    {
        AnimatorStateInfo currentStateInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextStateInfo = stateMachine.Animator.GetNextAnimatorStateInfo(0);

        if (stateMachine.Animator.IsInTransition(0) && nextStateInfo.IsTag("Attack"))
        {
            return nextStateInfo.normalizedTime;
        }
        else if(!stateMachine.Animator.IsInTransition(0) && currentStateInfo.IsTag("Attack"))
        {
            return currentStateInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }

}
