using Unity.VisualScripting;
using UnityEngine;

public abstract class E_State : StateBase
{
    public EnemyBrain EnemyBrain { get; protected set; }
    protected E_StateSO StateSo;
    
    protected E_State(EnemyBrain inBrain, E_StateSO inStateSo, StateMachineBase inStateMachine) : base(inBrain.gameObject, inStateMachine)
    {
        EnemyBrain = inBrain;
        StateSo = inStateSo;
        StateSo.Initialize(inBrain);
    }
    
    public override void Enter()
    {
        StateSo.Enter();
    }

    public override void Exit()
    {
        StateSo.Exit();
    }

    public override void Update()
    {
        StateSo.Update();
    }

    public override void FixedUpdate()
    {
        StateSo.FixedUpdate();
    }

    //When player is hit, allow each state to individualy decide what to do (generaly it will be to swap to hit state)
    public virtual void OnHit(bool isDead)
    {
        StateSo.OnHit(isDead);
    }
}