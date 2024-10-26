
using UnityEngine;
using UnityEngine.InputSystem;

public class PL_UserInterfaceStateMachine : StateMachineBase
{
    //CACHED COMPONENTS
    public ItemController ItemController {get; private set;}
    
    //INPUT
    public OnlyUIInput OnlyUIInput {get; private set;}
    public AlwaysActiveInput AlwaysActiveInput {get; private set;}
    
    //STATES
    public PL_DialogState PlDialogState {get; private set;}
    public PL_InventoryState PlInventoryState {get; private set;}
    public PL_PauseState PlPauseState {get; private set;}
    
    public PL_UserInterfaceStateMachine(GameObject inOwnerGameObject)
    {
        //CACHED COMPONENTS
        ItemController = inOwnerGameObject.GetComponent<ItemController>();
    
        //INPUT
        AlwaysActiveInput = GameManager.Instance.inputManager.alwaysActiveInput;
        OnlyUIInput = GameManager.Instance.inputManager.onlyUIInput;
        
        PlDialogState = new PL_DialogState(inOwnerGameObject, this);
        PlInventoryState = new PL_InventoryState(inOwnerGameObject, this);
        PlPauseState = new PL_PauseState(inOwnerGameObject, this);
    }

    public override void Initialize(StateBase startingState)
    {
        OnlyUIInput.enabled = true;
        
        AlwaysActiveInput.AlwaysActiveActions.Item0.performed += OnItem0Input;
        AlwaysActiveInput.AlwaysActiveActions.Item1.performed += OnItem1Input;
        AlwaysActiveInput.AlwaysActiveActions.Interact.performed += OnConfirmUI;
        
        OnlyUIInput.OnlyUiActions.ExtraConfirm.performed += OnConfirmUI;
        OnlyUIInput.OnlyUiActions.MouseItem0AndConfirm.performed += OnMouseItem0Input;
        OnlyUIInput.OnlyUiActions.MouseItem1AndConfirm.performed += OnMouseItem1Input;
        
        base.Initialize(startingState);
    }
    
    public void End()
    {
        AlwaysActiveInput.AlwaysActiveActions.Item0.performed -= OnItem0Input;
        AlwaysActiveInput.AlwaysActiveActions.Item1.performed -= OnItem1Input;
        AlwaysActiveInput.AlwaysActiveActions.Interact.performed -= OnConfirmUI;
        
        OnlyUIInput.OnlyUiActions.ExtraConfirm.performed -= OnConfirmUI;
        OnlyUIInput.OnlyUiActions.MouseItem0AndConfirm.performed -= OnMouseItem0Input;
        OnlyUIInput.OnlyUiActions.MouseItem1AndConfirm.performed -= OnMouseItem1Input;
        
        OnlyUIInput.enabled = false;
        
        CurrentState.Exit();
    }

    //Currently not being called from UserInterfaceState because its only a debug tool
    public override void Update()
    {
        base.Update();
        Debug.Log("Current UI State: " + CurrentState.ToString());
    }
    
    public void OnItem0Input(InputAction.CallbackContext context)
    {
        ((PL_UserInterfaceStateBase) CurrentState).OnItem0Input();
    }
    public void OnItem1Input(InputAction.CallbackContext context)
    {
        ((PL_UserInterfaceStateBase) CurrentState).OnItem1Input();
    }
    public void OnMouseItem0Input(InputAction.CallbackContext context)
    {
        ((PL_UserInterfaceStateBase) CurrentState).OnMouseItem0Input();
    }
    public void OnMouseItem1Input(InputAction.CallbackContext context)
    {
        ((PL_UserInterfaceStateBase) CurrentState).OnMouseItem1Input();
    }
    public void OnConfirmUI(InputAction.CallbackContext context)
    {
        ((PL_UserInterfaceStateBase) CurrentState).OnConfirmInput();
    }
    
}
