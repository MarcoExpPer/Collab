using System;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;


public abstract class PL_DebugStateBase : StateBase
{
    protected PL_DebugStateMachine PLdebugStateMachine;
    
    protected PL_DebugStateBase(GameObject inOwnerGameObject, PL_DebugStateMachine inStateMachine) : base(inOwnerGameObject, inStateMachine)
    {
        PLdebugStateMachine = inStateMachine;
    }

    public virtual void OnConfirmInput()
    {
       
    }

    public virtual void OnItem0Input(bool fromMouse, bool isPressed)
    {
        
    }

    public virtual void OnItem1Input(bool fromMouse, bool isPressed)
    {
     
    }

    public virtual void OnMoveInput(Vector2 moveInput)
    {
        
    }

}