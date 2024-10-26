
using UnityEngine;
using UnityEngine.InputSystem;

public class HitState : PL_GroundStateBase
{
    [SerializeField] FlashController flashController;
    
    
    public HitState(GameObject inOwnerGameObject, PL_GroundStateMachine inStateMachine) : base(inOwnerGameObject, inStateMachine)
    {
        flashController = inOwnerGameObject.GetComponent<FlashController>();
    }
    
    public override void Enter()
    { 
        DoHitVFX();
    }
    
    public override void Exit()
    {
        
    }
    
    public override void OnActionInput(EGroundActionInputType Action, InputAction.CallbackContext context){}

    public void DoHitVFX()
    {
        flashController.StartFlash();
    }

    public void OnPlayerHit()
    {
        
    }
}
