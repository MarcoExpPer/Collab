
using Interfaces;
using UnityEngine;


public class PL_DebugPanelState : PL_DebugStateBase
{
    public MainUIPanel MainPanel { get; protected set; }
    
    public PL_DebugPanelState(GameObject inOwnerGameObject, PL_DebugStateMachine inStateMachine) : base(inOwnerGameObject, inStateMachine)
    {
        MainPanel = GameManager.Instance.uiManager.debugPanel;
    }
    
    public override void Enter()
    {
        base.Enter();
        
        GameManager.Instance.uiManager.debugPanel.Toggle(true);
    }
    
    public override void Exit()
    {
        base.Exit();
        
        GameManager.Instance.uiManager.ActivePanels.Remove(GameManager.Instance.uiManager.debugPanel);
        GameManager.Instance.uiManager.debugPanel.Toggle(false);
    }
    
    public override void OnConfirmInput()
    {
        MainPanel.ConfirmInput(false);
    }

    public override void OnItem0Input(bool fromMouse, bool isPressed)
    {
        if (!isPressed) return;
        
        if (fromMouse)
        {
            if (!MainPanel.ItemInput(EEquipedItemSlot.Item0, true))
            {
                MainPanel.ConfirmInput(true);
            }
        }
        else
        {
            MainPanel.ItemInput(EEquipedItemSlot.Item0, false);
        }
       
    }

    public override void OnItem1Input(bool fromMouse, bool isPressed)
    {
        if (!isPressed) return;
        
        if (fromMouse)
        {
            if (!MainPanel.ItemInput(EEquipedItemSlot.Item1, true))
            {
                MainPanel.ConfirmInput(true);
            }
        }
        else
        {
            MainPanel.ItemInput(EEquipedItemSlot.Item1, false);
        }
    }
}