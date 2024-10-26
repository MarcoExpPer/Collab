using UnityEngine;

public class E_IdleState : E_State
{

    
    public E_IdleState(EnemyBrain inBrain, E_IdleStateSO inStateSo, StateMachineBase inStateMachine) : base(inBrain, inStateSo, inStateMachine)
    {
        
    }
    
    public override void Enter()
    {
        StateSo.Enter();
        
        EnemyBrain.enemySensesUpdated.AddListener(OnEnemySensesUpdated);
        EnemyBrain.SetActiveSenses(true);
    }

    public override void Exit()
    {
        StateSo.Exit();
        
        EnemyBrain.SetActiveSenses(false);
        EnemyBrain.enemySensesUpdated.RemoveListener(OnEnemySensesUpdated);
    }
    
    protected virtual void OnEnemySensesUpdated(EEnemySenseResult result, Vector3 sensePosition)
    {
        switch (result)
        {
            case EEnemySenseResult.Success:{
                //CHANGE TO IN COMBAT
                StateMachine.ChangeState(EnemyBrain.InCombatComplexState);
                break;
            }
            case EEnemySenseResult.Unsure:{
                //CHANGE TO ALERT
                StateMachine.ChangeState(EnemyBrain.AlertState);
                break;
            }
            case EEnemySenseResult.No:
                //DO NOTHING
                break;
        }
    }
}