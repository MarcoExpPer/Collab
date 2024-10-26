
using UnityEngine;
using UnityEngine.InputSystem;


public class PL_GroundState : StateBase
{
    private readonly PlayerSMController _playerSmController;
    private PL_GroundStateMachine _plGroundStateMachine;
    
    public PL_GroundState(PlayerSMController inPlayerSmController, StateMachineBase inStateMachine) : base(inPlayerSmController.gameObject, inStateMachine)
    {
        _playerSmController = inPlayerSmController;
        _plGroundStateMachine = new PL_GroundStateMachine(inPlayerSmController.gameObject);
    }

    public override void Enter()
    {
        _plGroundStateMachine.Initialize(_plGroundStateMachine.PlIdleState);
        _plGroundStateMachine.AlwaysActiveInput.AlwaysActiveActions.ToggleInventory.performed += OnOpenInventory;
        _plGroundStateMachine.AlwaysActiveInput.AlwaysActiveActions.ToggleDebug.performed += OnOpenDebugMenu;
    }

    public override void Update()
    {
        _plGroundStateMachine.Update();
    }
    
    public override void Exit()
    {
        _plGroundStateMachine.AlwaysActiveInput.AlwaysActiveActions.ToggleInventory.performed -= OnOpenInventory;
        _plGroundStateMachine.AlwaysActiveInput.AlwaysActiveActions.ToggleDebug.performed -= OnOpenDebugMenu;
        
        _plGroundStateMachine.End();
    }

    private void OnOpenInventory(InputAction.CallbackContext context)
    {
        GameManager.Instance.uiManager.ActivePanels.Add(GameManager.Instance.uiManager.inventoryPanel);
        StateMachine.ChangeState(_playerSmController.PlUserInterfaceState);
    }

    private void OnOpenDebugMenu(InputAction.CallbackContext context)
    {
        _playerSmController.PlDebugState.SetDebugState(EPL_DebugStates.PANEL);
        StateMachine.ChangeState(_playerSmController.PlDebugState);
    }
}
