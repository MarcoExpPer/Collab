using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine.InputSystem;

public class PL_UserInterfaceState : StateBase
{
    private PL_UserInterfaceStateMachine _plUserInterfaceStateMachine;
    private readonly PlayerSMController _playerSmController;

    public PL_UserInterfaceState(PlayerSMController inPlayerSmController, StateMachineBase inStateMachine) : base(
        inPlayerSmController.gameObject, inStateMachine)
    {
        _playerSmController = inPlayerSmController;
        _plUserInterfaceStateMachine = new PL_UserInterfaceStateMachine(inPlayerSmController.gameObject);
    }

    public override void Enter()
    {
        if (GameManager.Instance.uiManager.ActivePanels.Last() == _plUserInterfaceStateMachine.PlInventoryState.MainPanel)
        {
            _plUserInterfaceStateMachine.Initialize(_plUserInterfaceStateMachine.PlInventoryState);
        }
        else if (GameManager.Instance.uiManager.ActivePanels.Last() == _plUserInterfaceStateMachine.PlDialogState.MainPanel)
        {
            _plUserInterfaceStateMachine.Initialize(_plUserInterfaceStateMachine.PlDialogState);
        }
        else
        {
            _plUserInterfaceStateMachine.Initialize(_plUserInterfaceStateMachine.PlPauseState);
        }

        GameManager.Instance.uiManager.dialoguePanel.onDialogueFinished.AddListener(OnDialogFinished);
        _plUserInterfaceStateMachine.AlwaysActiveInput.AlwaysActiveActions.ToggleInventory.performed += ToggleInventory;
    }

    public override void Exit()
    {
        GameManager.Instance.uiManager.dialoguePanel.onDialogueFinished.RemoveListener(OnDialogFinished);
        _plUserInterfaceStateMachine.AlwaysActiveInput.AlwaysActiveActions.ToggleInventory.performed -= ToggleInventory;
        
        _plUserInterfaceStateMachine.End();
    }

    /**
     * If the inventory was open, close it.
     *      - If it was open and no more UI was open, return to ground state
     *      - Otherwise change to the state of the previously top panel
     *
     * If the inventory was not active, open it.
     */
    private void ToggleInventory(InputAction.CallbackContext context)
    {
        if (_plUserInterfaceStateMachine.PlInventoryState == _plUserInterfaceStateMachine.CurrentState)
        {
            GameManager.Instance.uiManager.ActivePanels.Remove(_plUserInterfaceStateMachine.PlInventoryState.MainPanel);
        }

        OnUIUpdated();
    }

    /**
     * If the dialog was the only UI open, return to ground state.
     * Otherwise change state to that other open ui
     */
    private void OnDialogFinished()
    {        
        GameManager.Instance.uiManager.ActivePanels.Remove(_plUserInterfaceStateMachine.PlDialogState.MainPanel);
        
        OnUIUpdated();
    }

    private void OnUIUpdated()
    {
        List<MainUIPanel> ActivePanels = GameManager.Instance.uiManager.ActivePanels;

        if (ActivePanels.Count == 0)
        {
            StateMachine.ChangeState(_playerSmController.PlGroundState);
            
            //If Inventory is main panel  
        }else if (ActivePanels.Last() == _plUserInterfaceStateMachine.PlInventoryState.MainPanel)
        {
            _plUserInterfaceStateMachine.ChangeState(_plUserInterfaceStateMachine.PlInventoryState);
            
            //If Dialog is main panel
        }else if (ActivePanels.Last() == _plUserInterfaceStateMachine.PlDialogState.MainPanel)
        {
            _plUserInterfaceStateMachine.ChangeState(_plUserInterfaceStateMachine.PlDialogState);
            
        }   //If pause is main panel
        else if (ActivePanels.Last() == _plUserInterfaceStateMachine.PlPauseState.MainPanel)
        {
            //TODO
        }
    }
}