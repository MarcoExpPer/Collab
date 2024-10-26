
using UnityEngine;


public class PL_DialogState : PL_UserInterfaceStateBase
{
    public PL_DialogState(GameObject inOwnerGameObject, PL_UserInterfaceStateMachine inStateMachine) : base(inOwnerGameObject, inStateMachine)
    {
        MainPanel = GameManager.Instance.uiManager.dialoguePanel;
    }
    
    public override void Enter()
    {
        base.Enter();
        
        GameManager.Instance.uiManager.dialoguePanel.Toggle(true);
    }
    
    public override void Exit()
    {
        base.Exit();
        

        GameManager.Instance.uiManager.ActivePanels.Remove(GameManager.Instance.uiManager.dialoguePanel);
        GameManager.Instance.uiManager.inventoryPanel.Toggle(false);
    }
}