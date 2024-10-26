
using System;
using UnityEngine;

public class PL_DraggingState : PL_GroundStateBase
{
    private GroundMovementController _movementController;
    public PL_DraggingState(GameObject inOwnerGameObject, PL_GroundStateMachine inStateMachine) : base(inOwnerGameObject, inStateMachine)
    {
        _movementController = inOwnerGameObject.GetComponent<GroundMovementController>();
    }
    
    public override void Enter()
    {
        _movementController.enabled = true;
        PlGroundStateMachine.Animator.SetBool(PlayerSMController.IsDraggingParam, true);
    }

    public override void Exit()
    {
        _movementController.enabled = false;
        PlGroundStateMachine.Animator.SetBool(PlayerSMController.IsDraggingParam, false);
    }
    

    public override void OnMoveInput(Vector2 moveInput)
    {
        if (moveInput != Vector2.zero || !Mathf.Approximately(_movementController.Controller.velocity.sqrMagnitude, 0f))
        {
            _movementController.DragObject(moveInput);
        }
        
        if(!_movementController.isDragging())
        {
            StateMachine.ChangeState(PlGroundStateMachine.PlIdleState); 
        }
        
    }
}
