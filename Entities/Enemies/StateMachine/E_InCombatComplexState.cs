using UnityEngine;

public class E_InCombatComplexState : E_State
{
    [Header("Combat States")] 
    public StateMachineBase InCombatStateMachine;
    public EAttackStateBase AttackStateBase;
    public ECombatMovementStateBase CombatMovementStateBase;

    public E_InCombatStateSO InCombatStateSO;
    private float timeWithoutSenses;
    
    public E_InCombatComplexState(EnemyBrain inBrain, E_InCombatStateSO inStateSo, StateMachineBase inStateMachine) : base(inBrain, inStateSo, inStateMachine)
    {
        InCombatStateMachine = new StateMachineBase();
        InCombatStateMachine.enableDebug = inBrain.enableSmDebug;
        
        AttackStateBase = new EAttackStateBase(this, Object.Instantiate(inStateSo.attackStateSo) , InCombatStateMachine);
        CombatMovementStateBase = new ECombatMovementStateBase(this, Object.Instantiate(inStateSo.combatMovementStateSO), InCombatStateMachine);
        InCombatStateSO = inStateSo;
        
        foreach (E_AttackCauseSO attackCause in inStateSo.attackCausesInstances)
        {
            attackCause.OnRuntimeSetup(inBrain);
        }
        
        inStateSo.shouldAttack.AddListener(OnShouldAttackEvent);
    }


    public override void Enter()
    {
        base.Enter();
        timeWithoutSenses = 1;
        EnemyBrain.ReturnToIdleState.returnToidleSO.playerTrigger.enabled = true;
        InCombatStateMachine.Initialize(CombatMovementStateBase);
        
        EnemyBrain.enemySensesUpdated.AddListener(OnEnemySensesUpdated);
        EnemyBrain.SetActiveSenses(true);
    }

    public override void Update()
    {
        base.Update();
        
        InCombatStateMachine.Update();

        if (timeWithoutSenses != -1)
        {
            timeWithoutSenses += Time.deltaTime;
            if (timeWithoutSenses > InCombatStateSO.timeWithoutSenseToReturnToIdle)
            {
                InCombatStateMachine.ChangeState(EnemyBrain.ReturnToIdleState);
            }
        }

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        InCombatStateMachine.FixedUpdate();
    }
    
    public override void Exit()
    {
        base.Exit();
        
        EnemyBrain.ReturnToIdleState.returnToidleSO.playerTrigger.enabled = false;
        InCombatStateMachine.CurrentState.Exit();
        
        EnemyBrain.SetActiveSenses(false);
        EnemyBrain.enemySensesUpdated.RemoveListener(OnEnemySensesUpdated);
    }

    public void OnShouldAttackEvent()
    {
        if (InCombatStateMachine.CurrentState != AttackStateBase)
        {
            InCombatStateMachine.ChangeState(AttackStateBase);
        }
    }

    public bool ManualAttackCausesQuery()
    {
        return InCombatStateSO.DoQueryAttackCauses();
    }
    
    protected virtual void OnEnemySensesUpdated(EEnemySenseResult result, Vector3 sensePosition)
    {
        switch (result)
        {
            case EEnemySenseResult.Success:
            {
                timeWithoutSenses = -1;
                break;
            }
            case EEnemySenseResult.Unsure:
            {
                timeWithoutSenses = -1;
                break;
            }
            case EEnemySenseResult.No:
                //DO NOTHING
                if (timeWithoutSenses == -1)
                {
                    timeWithoutSenses = 0;
                }
                break;
        }
    }


    public override void OnHit(bool isDead)
    {
        //If we are not in an attacking state, we change to hit state
        if (InCombatStateMachine.CurrentState != AttackStateBase)
        {
            EnemyBrain.StateMachine.ChangeState(EnemyBrain.HitState);
        }
        else
        {
            //Otherwise we let the current attack action decide in the inCombatStateSO
            base.OnHit(isDead);
        }
        
    }
}