using System;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;


public abstract class PL_UserInterfaceStateBase : StateBase
{
    protected PL_UserInterfaceStateMachine PlUserInterfaceStateMachine;
    public MainUIPanel MainPanel { get; protected set; }
    
    protected PL_UserInterfaceStateBase(GameObject inOwnerGameObject, PL_UserInterfaceStateMachine inStateMachine) : base(inOwnerGameObject, inStateMachine)
    {
        PlUserInterfaceStateMachine = inStateMachine;
    }

    public virtual void OnConfirmInput()
    {
        MainPanel.ConfirmInput(false);
    }

    public virtual void OnItem0Input()
    {
        MainPanel.ItemInput(EEquipedItemSlot.Item0, false);
    }

    public virtual void OnItem1Input()
    {
        MainPanel.ItemInput(EEquipedItemSlot.Item1, false);
    }

    public virtual void OnMouseItem0Input()
    {
        if (!MainPanel.ItemInput(EEquipedItemSlot.Item0, true))
        {
            MainPanel.ConfirmInput(true);
        }
    }

    public virtual void OnMouseItem1Input()
    {
        if (!MainPanel.ItemInput(EEquipedItemSlot.Item1, true))
        {
            MainPanel.ConfirmInput(true);
        }
    }
    
}