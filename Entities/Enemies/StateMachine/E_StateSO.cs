
using UnityEngine;

public abstract class E_StateSO : ScriptableObject
{
    [SerializeField] protected EnemyBrain EnemyBrain;

    public virtual void Initialize(EnemyBrain inBrain)
    {
        EnemyBrain = inBrain;
    }

    public virtual void OnValidateSetup(EnemyBrain inBrain)
    {
        
    }

    public virtual void Enter()
    {
        
    }

    public virtual void Update()
    {
        
    }
    
    public virtual void FixedUpdate()
    {
        
    }
    
    public virtual void Exit()
    {
        
    }

    public virtual void OnHit(bool isDead)
    {
        EnemyBrain.StateMachine.ChangeState(EnemyBrain.HitState);
    }
}
