
using UnityEngine;


public class PL_DebugFlyState : PL_DebugStateBase
{
    private Animator _animator;
    private GroundMovementController _movementController;

    private bool isUpPressed = false;
    private bool isDownPressed = false;
    
    public PL_DebugFlyState(GameObject inOwnerGameObject, PL_DebugStateMachine inStateMachine) : base(inOwnerGameObject, inStateMachine)
    {
        _animator = inOwnerGameObject.GetComponent<Animator>();
        _movementController = inOwnerGameObject.GetComponent<GroundMovementController>();
    }

    public override void Enter()
    {
        base.Enter();
        
        _animator.applyRootMotion = false;
        PLdebugStateMachine.canExitDebug += 1;
    }
    
    public override void Exit()
    {
        PLdebugStateMachine.canExitDebug -= 1;
        _animator.applyRootMotion = true;
        base.Exit();
    }

    public override void OnMoveInput(Vector2 moveInput)
    {
        float verticalMovement = isUpPressed ? 1 : isDownPressed ? -1 : 0;
        Vector3 movement = new Vector3(moveInput.x, verticalMovement, moveInput.y);
        
        _animator.SetBool(PlayerSMController.IsMovingParam, movement.sqrMagnitude > 0);
        _movementController.Fly(movement);
    }
    
    public override void OnItem0Input(bool fromMouse, bool isPressed)
    {
        if (fromMouse) return;
        
        isDownPressed = isPressed;
    }

    public override void OnItem1Input(bool fromMouse, bool isPressed)
    {
        if (fromMouse) return;

        isUpPressed = isPressed;
    }
}