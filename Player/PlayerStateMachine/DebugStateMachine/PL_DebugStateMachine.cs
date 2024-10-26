
using UnityEngine;
using UnityEngine.InputSystem;


public class PL_DebugStateMachine : StateMachineBase
{
    //STATES
    public PL_DebugFlyState DebugFlyState;
    public PL_DebugPanelState DebugPanelState;
    
    //INPUT
    public GroundedInput GroundedInput {get; private set;}
    public AlwaysActiveInput AlwaysActiveInput {get; private set;}
    public OnlyUIInput OnlyUIInput {get; private set;}
    
    //Increase by one when something should impede returning to normal mode (like flying mode). Reduce when not. 0 means its okey to return
    public int canExitDebug = 0;
    public PL_DebugStateMachine(GameObject inOwnerGameObject)
    {
        //INPUT
        AlwaysActiveInput = GameManager.Instance.inputManager.alwaysActiveInput;
        GroundedInput = GameManager.Instance.inputManager.groundedInput;
        OnlyUIInput = GameManager.Instance.inputManager.onlyUIInput;
        
        //STATES
        DebugFlyState = new PL_DebugFlyState(inOwnerGameObject, this);
        DebugPanelState = new PL_DebugPanelState(inOwnerGameObject, this);
    }

    public override void Initialize(StateBase startingState)
    {
        GroundedInput.enabled = true;
        OnlyUIInput.enabled = true;
        
        AlwaysActiveInput.AlwaysActiveActions.Item0.performed += OnItem0Input;
        AlwaysActiveInput.AlwaysActiveActions.Item1.performed += OnItem1Input;
        AlwaysActiveInput.AlwaysActiveActions.Item0.canceled += OnItem0Input;
        AlwaysActiveInput.AlwaysActiveActions.Item1.canceled += OnItem1Input;
        
        AlwaysActiveInput.AlwaysActiveActions.Interact.performed += OnConfirmUI;
        
        OnlyUIInput.OnlyUiActions.ExtraConfirm.performed += OnConfirmUI;
        OnlyUIInput.OnlyUiActions.MouseItem0AndConfirm.performed += OnItem0InputMouse;
        OnlyUIInput.OnlyUiActions.MouseItem1AndConfirm.performed += OnItem1InputMouse;
        
        base.Initialize(startingState);
    }
    
    public void End()
    {
       
        AlwaysActiveInput.AlwaysActiveActions.Item0.performed -= OnItem0Input;
        AlwaysActiveInput.AlwaysActiveActions.Item1.performed -= OnItem1Input;
        AlwaysActiveInput.AlwaysActiveActions.Item0.canceled -= OnItem0Input;
        AlwaysActiveInput.AlwaysActiveActions.Item1.canceled -= OnItem1Input;
        
        AlwaysActiveInput.AlwaysActiveActions.Interact.performed -= OnConfirmUI;
        
        OnlyUIInput.OnlyUiActions.ExtraConfirm.performed -= OnConfirmUI;
        OnlyUIInput.OnlyUiActions.MouseItem0AndConfirm.performed -= OnItem0InputMouse;
        OnlyUIInput.OnlyUiActions.MouseItem1AndConfirm.performed -= OnItem1InputMouse;
        
        GroundedInput.enabled = false;
        OnlyUIInput.enabled = false;
        
        CurrentState.Exit();
    }

    //Currently not being called from UserInterfaceState because its only a debug tool
    public override void Update()
    {
        CurrentState.Update();
        
        ((PL_DebugStateBase) CurrentState).OnMoveInput(GroundedInput.CurrentMoveInput);
    }

    public void ForceChangeState(EPL_DebugStates inDebugState)
    {
        StateBase newState = inDebugState == EPL_DebugStates.FLY ? DebugFlyState : DebugPanelState;
        CurrentState = newState;
    }
    
    private void OnItem0Input(InputAction.CallbackContext context)
    {
        ((PL_DebugStateBase) CurrentState).OnItem0Input(false, context.ReadValueAsButton());
    }
    private void OnItem1Input(InputAction.CallbackContext context)
    {
        ((PL_DebugStateBase) CurrentState).OnItem1Input(false, context.ReadValueAsButton());
    }
    
    private void OnItem0InputMouse(InputAction.CallbackContext context)
    {
        ((PL_DebugStateBase) CurrentState).OnItem0Input(true, context.ReadValueAsButton());
    }

    private void OnItem1InputMouse(InputAction.CallbackContext context)
    {
        ((PL_DebugStateBase) CurrentState).OnItem1Input(true, context.ReadValueAsButton());
    }
    private void OnConfirmUI(InputAction.CallbackContext context)
    {
        ((PL_DebugStateBase) CurrentState).OnConfirmInput();
    }
}
