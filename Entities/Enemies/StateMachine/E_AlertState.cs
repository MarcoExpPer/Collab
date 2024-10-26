using UnityEngine;

public class E_AlertState : E_State
{
    private E_AlertStateSO _alertStateSO;
    private Vector3 lastSenseLocation;
    
    //-1 means the timer is stopped
    public float currentMissedSenseTime = -1;
    
    public E_AlertState(EnemyBrain inBrain, E_AlertStateSO inStateSo, StateMachineBase inStateMachine) : base(inBrain, inStateSo, inStateMachine)
    {
        _alertStateSO = inStateSo;
    }
    
    public override void Enter()
    {
        currentMissedSenseTime = -1;
        StateSo.Enter();
        
        EnemyBrain.enemySensesUpdated.AddListener(OnEnemySensesUpdated);
        EnemyBrain.SetActiveSenses(true);

        var senseResults = EnemyBrain.QuerySenses();
        OnEnemySensesUpdated(senseResults.Item1, senseResults.Item2);
    }

    public override void Exit()
    {
        StateSo.Exit();
        
        EnemyBrain.SetActiveSenses(false);
        EnemyBrain.enemySensesUpdated.RemoveListener(OnEnemySensesUpdated);
        
    }

    public override void Update()
    {
        if (_alertStateSO.lookAtSense)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lastSenseLocation - EnemyBrain.transform.position, Vector3.up);
            Quaternion newRotation = Quaternion.Lerp(EnemyBrain.transform.rotation, targetRotation, Time.deltaTime * _alertStateSO.lookAtRotationSpeed);
            EnemyBrain.transform.rotation = newRotation;
        }

        if (currentMissedSenseTime != -1)
        {
            currentMissedSenseTime += Time.deltaTime;

            if (currentMissedSenseTime > _alertStateSO.mantainAlertAfterSenseLost)
            {
                StateMachine.ChangeState(EnemyBrain.IdleState);
            }
        }
        else
        {
            //We dont want ALertStateSO timer to "change to in combat" player
            StateSo.Update();
        }
    }

    public override void FixedUpdate()
    {
        StateSo.FixedUpdate();
    }

    //When player is hit, allow each state to individualy decide what to do (generaly it will be to swap to hit state)
    public override void OnHit(bool isDead)
    {
        StateSo.OnHit(isDead);
    }
    
    protected virtual void OnEnemySensesUpdated(EEnemySenseResult result, Vector3 senseLocation)
    {
        switch (result)
        {
            case EEnemySenseResult.Success:
                //CHANGE TO IN COMBAT
                StateMachine.ChangeState(EnemyBrain.InCombatComplexState);
                currentMissedSenseTime = -1;
                break;
            case EEnemySenseResult.Unsure:
                //KEEP WAITING
                currentMissedSenseTime = -1;
                lastSenseLocation = senseLocation;
                break;
            case EEnemySenseResult.No:
                //CHANGE TO RETURN TO IDLE / TO IDLE
                if (_alertStateSO.mantainAlertAfterSenseLost > 0 )
                {
                    if (currentMissedSenseTime == -1)
                    {
                        currentMissedSenseTime = 0;
                    }
                }
                else
                {
                    StateMachine.ChangeState(EnemyBrain.IdleState);
                }
                
                break;
        }
    }
}