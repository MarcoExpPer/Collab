
using UnityEngine.InputSystem;

public enum EPL_DebugStates
{
    PANEL,
    FLY
}

public class PL_DebugState : StateBase
{
    private readonly PlayerSMController _playerSmController;
    public PL_DebugStateMachine PlDebugStateMachine;
    public AlwaysActiveInput AlwaysActiveInput {get; private set;}

    
    public PL_DebugState(PlayerSMController inPlayerSmController, StateMachineBase inStateMachine) : base(inPlayerSmController.gameObject, inStateMachine)
    {
        _playerSmController = inPlayerSmController;
        PlDebugStateMachine = new PL_DebugStateMachine(inPlayerSmController.gameObject);
        
        AlwaysActiveInput = GameManager.Instance.inputManager.alwaysActiveInput;
    }

    public override void Enter()
    {
        PlDebugStateMachine.Initialize(PlDebugStateMachine.CurrentState);

        AlwaysActiveInput.AlwaysActiveActions.ToggleDebug.performed += ToggleDebug;
    }

    public override void Update()
    {
        PlDebugStateMachine.Update();
    }
    
    public override void Exit()
    {
        AlwaysActiveInput.AlwaysActiveActions.ToggleDebug.performed -= ToggleDebug;
        
        PlDebugStateMachine.End();
    }

    public void SetDebugState(EPL_DebugStates inDebugState)
    {
        PlDebugStateMachine.ForceChangeState(inDebugState);
    }

    public void ToggleDebug(InputAction.CallbackContext context)
    {
        if (PlDebugStateMachine.CurrentState == PlDebugStateMachine.DebugPanelState)
        {
            //Si queremos salir del estado de debug, hay que comprobar que podamos hacerlo
            if (PlDebugStateMachine.canExitDebug == 0)
            {
                StateMachine.ChangeState(_playerSmController.PlGroundState);
            }
            else
            {
                PlDebugStateMachine.ChangeState(StateMachine.PreviousState);
            }
        }
        else
        {
            PlDebugStateMachine.ChangeState(PlDebugStateMachine.DebugPanelState);
        }
    }

}