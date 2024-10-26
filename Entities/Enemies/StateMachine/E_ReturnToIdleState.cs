using UnityEngine;
using UnityEngine.AI;

public class E_ReturnToIdleState : E_State
{
    private NavMeshAgent agent;
    private Vector3 initialPos;
    public E_ReturnToIdleStateSO returnToidleSO;
    
    private float currentTime;
    public E_ReturnToIdleState(EnemyBrain inBrain, E_ReturnToIdleStateSO inStateSo, StateMachineBase inStateMachine) : base(inBrain, inStateSo, inStateMachine)
    {
        agent = inBrain.navMeshAgent;
        initialPos = inBrain.transform.position;
        returnToidleSO = inStateSo;
    }
    
    public override void Enter()
    {
        agent.isStopped = false;
        agent.updateRotation = true;
        agent.SetDestination(initialPos);
        currentTime = 0;
        
        EnemyBrain.enemySensesUpdated.AddListener(OnEnemySensesUpdated);
        EnemyBrain.SetActiveSenses(true);
    }

    public override void Exit()
    {
        agent.isStopped = true;
        agent.updateRotation = false;
        
        EnemyBrain.SetActiveSenses(false);
        EnemyBrain.enemySensesUpdated.RemoveListener(OnEnemySensesUpdated);
    }

    public override void Update()
    {
        currentTime += Time.deltaTime;
        
        if ( !agent.pathPending && !agent.hasPath)
        {
            EnemyBrain.StateMachine.ChangeState(EnemyBrain.IdleState);
        }
    }
    
    protected virtual void OnEnemySensesUpdated(EEnemySenseResult result, Vector3 sensePosition)
    {
        if (currentTime <= returnToidleSO.ignoreSensesTime)
        {
            return;
        }

        if (result == EEnemySenseResult.Success)
        {
            EnemyBrain.StateMachine.ChangeState(EnemyBrain.InCombatComplexState);
        }
    }
}