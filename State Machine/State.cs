using UnityEngine;

public abstract class StateBase
{
    protected GameObject OwnerGameObject;
    protected StateMachineBase StateMachine;
    protected StateBase(GameObject inOwnerGameObject, StateMachineBase inStateMachine)
    {
        OwnerGameObject = inOwnerGameObject;
        StateMachine = inStateMachine;
    }

    public virtual void Enter()
    {
        
    }

    public virtual void Exit()
    {
        
    }

    public virtual void Update()
    {
        
    }
    
    public virtual void FixedUpdate()
    {
        
    }
}