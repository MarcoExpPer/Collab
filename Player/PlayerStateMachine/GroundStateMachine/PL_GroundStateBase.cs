using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EGroundActionInputType
{
    Interaction,
    Item0,
    Item1,
    Sword,
    Shield
}

public abstract class PL_GroundStateBase : StateBase
{
    protected PL_GroundStateMachine PlGroundStateMachine;
    
    protected PL_GroundStateBase(GameObject inOwnerGameObject, PL_GroundStateMachine inStateMachine) : base(inOwnerGameObject, inStateMachine)
    {
        PlGroundStateMachine = inStateMachine;
    }

    public virtual void OnMoveInput(Vector2 moveInput){}
    
    public virtual void OnActionInput(EGroundActionInputType Action, InputAction.CallbackContext context)
    {
        switch (Action)
        {
            case EGroundActionInputType.Interaction:
                if (context.ReadValueAsButton())
                {
                    PlGroundStateMachine.InteractionController.Interact(); 
                }
                break;
            case EGroundActionInputType.Item0:
                PlGroundStateMachine.ItemController.ItemInput(context.ReadValueAsButton(), EEquipedItemSlot.Item0);
                break;
            case EGroundActionInputType.Item1:
                PlGroundStateMachine.ItemController.ItemInput(context.ReadValueAsButton(), EEquipedItemSlot.Item1);
                break;
            case EGroundActionInputType.Sword:
                PlGroundStateMachine.ItemController.ItemInput(context.ReadValueAsButton(), EEquipedItemSlot.Sword);
                break;
            case EGroundActionInputType.Shield:
                PlGroundStateMachine.ItemController.ItemInput(context.ReadValueAsButton(), EEquipedItemSlot.Shield);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(Action), Action, null);
        }
    }
    
}