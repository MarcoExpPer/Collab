using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using static InputActions;

public class OnlyUIInput : MonoBehaviour
{
    public OnlyUIActions OnlyUiActions { get; private set; }
    public UnityEvent OnEnableEvent { get; private set; } = new UnityEvent();
    public UnityEvent OnDisableEvent { get; private set; } = new UnityEvent();
    
    private void Awake()
    {
        OnlyUiActions = new InputActions().OnlyUI;
    }

    void OnEnable()
    {
        OnlyUiActions.Enable();
        OnEnableEvent.Invoke();
    }
    
    void OnDisable()
    { 
        OnlyUiActions.Disable();
        OnDisableEvent.Invoke();
    }
}