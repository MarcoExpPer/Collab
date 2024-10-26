
using UnityEngine;


public class PL_InventoryState : PL_UserInterfaceStateBase
{
    public PL_InventoryState(GameObject inOwnerGameObject, PL_UserInterfaceStateMachine inStateMachine) : base(inOwnerGameObject, inStateMachine)
    {
        MainPanel = GameManager.Instance.uiManager.inventoryPanel;
    }

    public override void Enter()
    {
        base.Enter();
        
        GameManager.Instance.uiManager.inventoryPanel.Toggle(true);
    }
    
    public override void Exit()
    {
        base.Exit();
        
        GameManager.Instance.uiManager.ActivePanels.Remove(GameManager.Instance.uiManager.inventoryPanel);
        GameManager.Instance.uiManager.inventoryPanel.Toggle(false);
    }
}