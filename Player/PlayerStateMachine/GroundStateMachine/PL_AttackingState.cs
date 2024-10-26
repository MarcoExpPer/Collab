
using UnityEngine;
using UnityEngine.InputSystem;

public class PL_AttackingState : PL_GroundStateBase
{
    public PL_AttackingState(GameObject inOwnerGameObject, PL_GroundStateMachine inStateMachine) : base(inOwnerGameObject, inStateMachine)
    {
    }
    
    public override void Enter()
    {
       
    }

    public override void Exit()
    {
        
    }
    
    public override void OnActionInput(EGroundActionInputType Action, InputAction.CallbackContext context){}
}
