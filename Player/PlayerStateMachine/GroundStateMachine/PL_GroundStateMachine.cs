
using UnityEngine;
using UnityEngine.InputSystem;

public class PL_GroundStateMachine : StateMachineBase
{
    //CACHED COMPONENTS
    public GroundMovementController MovementController {get; private set;}
    public ItemController ItemController {get; private set;}
    public InteractionController InteractionController {get; private set;}
    public Animator Animator {get; private set;}
    
    //INPUT
    public GroundedInput GroundedInput {get; private set;}
    public AlwaysActiveInput AlwaysActiveInput {get; private set;}
    
    //STATES
    public PL_WalkingState PlWalkingState;
    public HitState HitState;
    public PL_IdleState PlIdleState;
    public PL_AttackingState PlAttackingState;
    public PL_DraggingState PlDraggingState;
    
    public PL_GroundStateMachine(GameObject inOwnerGameObject)
    {
        //CACHED COMPONENTS
        MovementController = inOwnerGameObject.GetComponent<GroundMovementController>();
        ItemController = inOwnerGameObject.GetComponent<ItemController>();
        Animator = inOwnerGameObject.GetComponent<Animator>();
        InteractionController = inOwnerGameObject.transform.GetComponentInChildren<InteractionController>(); 
    
        //INPUT
        AlwaysActiveInput = GameManager.Instance.inputManager.alwaysActiveInput;
        GroundedInput = GameManager.Instance.inputManager.groundedInput;
        
        PlWalkingState = new PL_WalkingState(inOwnerGameObject, this);
        HitState = new HitState(inOwnerGameObject, this);
        PlIdleState = new PL_IdleState(inOwnerGameObject, this);
        PlAttackingState = new PL_AttackingState(inOwnerGameObject, this);
        PlDraggingState = new PL_DraggingState(inOwnerGameObject, this);
    }

    public override void Initialize(StateBase startingState)
    {
        GroundedInput.enabled = true;
        //performed
        AlwaysActiveInput.AlwaysActiveActions.Item0.performed += OnItem0Input;
        AlwaysActiveInput.AlwaysActiveActions.Item1.performed += OnItem1Input;
        AlwaysActiveInput.AlwaysActiveActions.Interact.performed += OnInteractInput;
        GroundedInput.GroundedActions.Sword.performed += OnSwordInput;
        GroundedInput.GroundedActions.Shield.performed += OnShieldInput;
        
        //Canceled
        AlwaysActiveInput.AlwaysActiveActions.Item0.canceled += OnItem0Input;
        AlwaysActiveInput.AlwaysActiveActions.Item1.canceled += OnItem1Input;
        AlwaysActiveInput.AlwaysActiveActions.Interact.canceled += OnInteractInput;
        GroundedInput.GroundedActions.Sword.canceled += OnSwordInput;
        GroundedInput.GroundedActions.Shield.canceled += OnShieldInput;
        
        base.Initialize(startingState);
    }
    
    public void End()
    {
        
        AlwaysActiveInput.AlwaysActiveActions.Item0.performed -= OnItem0Input;
        AlwaysActiveInput.AlwaysActiveActions.Item1.performed -= OnItem1Input;
        AlwaysActiveInput.AlwaysActiveActions.Interact.performed -= OnInteractInput;
        
        AlwaysActiveInput.AlwaysActiveActions.Item0.canceled -= OnItem0Input;
        AlwaysActiveInput.AlwaysActiveActions.Item1.canceled -= OnItem1Input;
        AlwaysActiveInput.AlwaysActiveActions.Interact.canceled -= OnInteractInput;

        if (GroundedInput)
        {
            GroundedInput.GroundedActions.Sword.performed -= OnSwordInput;
            GroundedInput.GroundedActions.Shield.performed -= OnShieldInput;
            GroundedInput.GroundedActions.Sword.canceled -= OnSwordInput;
            GroundedInput.GroundedActions.Shield.canceled -= OnShieldInput;

            GroundedInput.enabled = false;
        }

        CurrentState.Exit();
    }

    public override void Update()
    {
        base.Update();
        
        ((PL_GroundStateBase) CurrentState).OnMoveInput(GroundedInput.CurrentMoveInput);
    }

    public void OnItem0Input(InputAction.CallbackContext context)
    {
        ((PL_GroundStateBase) CurrentState).OnActionInput(EGroundActionInputType.Item0, context);
    }
    public void OnItem1Input(InputAction.CallbackContext context)
    {
        ((PL_GroundStateBase) CurrentState).OnActionInput(EGroundActionInputType.Item1, context);
    }
    public void OnSwordInput(InputAction.CallbackContext context)
    {
        ((PL_GroundStateBase) CurrentState).OnActionInput(EGroundActionInputType.Sword, context);
    }
    public void OnShieldInput(InputAction.CallbackContext context)
    {
        ((PL_GroundStateBase) CurrentState).OnActionInput(EGroundActionInputType.Shield, context);
    }
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        ((PL_GroundStateBase) CurrentState).OnActionInput(EGroundActionInputType.Interaction, context);
    }
}
