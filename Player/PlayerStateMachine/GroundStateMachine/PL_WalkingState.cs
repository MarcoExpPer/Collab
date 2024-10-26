
using System;
using UnityEngine;

public class PL_WalkingState : PL_GroundStateBase
{
    private GroundMovementController _movementController;
    public PL_WalkingState(GameObject inOwnerGameObject, PL_GroundStateMachine inStateMachine) : base(inOwnerGameObject, inStateMachine)
    {
        _movementController = inOwnerGameObject.GetComponent<GroundMovementController>();
    }
    
    public override void Enter()
    {
        _movementController.enabled = true;
        PlGroundStateMachine.Animator.SetBool(PlayerSMController.IsMovingParam, true);
    }

    public override void Exit()
    {
        _movementController.enabled = false;
        PlGroundStateMachine.Animator.SetBool(PlayerSMController.IsMovingParam, false);
    }
    
    //Called in GroundStateMachine update
    public override void OnMoveInput(Vector2 moveInput)
    {
        if (moveInput == Vector2.zero && Mathf.Approximately(_movementController.Controller.velocity.sqrMagnitude, 0f))
        {
            PlGroundStateMachine.ChangeState(PlGroundStateMachine.PlIdleState);
        }
        else
        {
            _movementController.Walk(moveInput);
        }

        if(_movementController.isDragging())
        {
            StateMachine.ChangeState(PlGroundStateMachine.PlDraggingState); 
        }
    }
}
