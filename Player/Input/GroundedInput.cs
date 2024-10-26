using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

using static InputActions;

public class GroundedInput : MonoBehaviour
{
    public Vector2 CurrentMoveInput { get; private set; } = Vector2.zero;
    public GroundedActions GroundedActions { get; private set; }
    
    public UnityEvent OnEnableEvent { get;} = new UnityEvent();
    public UnityEvent OnDisableEvent { get; } = new UnityEvent();

    private void Awake()
    {
        GroundedActions = new InputActions().Grounded;

        if (enabled)
        {
            OnEnable();
        }
    }

    void OnEnable()
    {
        GroundedActions.Movement.performed += SetMove;
        GroundedActions.Movement.canceled += SetMove;
        
        GroundedActions.Enable();
        OnEnableEvent.Invoke();
    }
    
   public void SetMove(InputAction.CallbackContext context)
    {
        CurrentMoveInput = context.ReadValue<Vector2>();
    }
   
    void OnDisable()
    {
        GroundedActions.Movement.performed -= SetMove;
        GroundedActions.Movement.canceled -= SetMove;
        CurrentMoveInput = Vector2.zero;
        
        GroundedActions.Disable();
        OnDisableEvent.Invoke();
    }
}