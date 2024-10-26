
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PL_IdleState : PL_GroundStateBase
{
    
    public PL_IdleState(GameObject inOwnerGameObject, PL_GroundStateMachine inStateMachine) : base(inOwnerGameObject, inStateMachine)
    {
    }
    
    public override void Enter()
    {
        //PlGroundStateMachine.Animator.SetBool(PlayerSMController.IsMovingParam, false);
        //PlGroundStateMachine.Animator.SetBool(PlayerSMController.IsDraggingParam, false);
    }

    public override void Exit()
    {
        
    }

    public override void OnMoveInput(Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            StateMachine.ChangeState(PlGroundStateMachine.PlWalkingState); 
        }
    }
}
