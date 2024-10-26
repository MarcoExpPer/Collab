
using UnityEngine;
using UnityEngine.Events;

public class StateMachineBase
{
    public StateBase CurrentState { get; protected set; }
    public StateBase PreviousState { get; protected set; }
    //Must be enabled from the script that controls this state machine
    public bool enableDebug = false;
    //First previous state, second new state
    public UnityEvent<StateBase, StateBase> onStateChanged = new UnityEvent<StateBase, StateBase>();
    
    public virtual void Initialize(StateBase startingState)
    {
        CurrentState = startingState;
        PreviousState = CurrentState;
        CurrentState.Enter();
        
        if(enableDebug)
            Debug.Log( "Start SM as -> " + CurrentState.ToString());
    }

    public void ChangeState(StateBase newState)
    {
        if (CurrentState != newState)
        {
            PreviousState = CurrentState;
            CurrentState.Exit();
            
            CurrentState = newState;
            newState.Enter();
            
            if(enableDebug)
                Debug.Log(PreviousState.ToString() + " -> " + CurrentState.ToString());
            
            onStateChanged.Invoke(PreviousState, CurrentState);
        }
    }

    public virtual void Update()
    {
        CurrentState.Update();
    }
    
    public virtual void FixedUpdate()
    {
        CurrentState.FixedUpdate();
    }
}
