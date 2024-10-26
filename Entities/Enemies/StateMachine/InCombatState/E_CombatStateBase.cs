using UnityEngine;

public abstract class E_CombatStateBase : E_State
{
    protected E_InCombatComplexState InCombatComplexState;
    
    protected E_CombatStateBase(E_InCombatComplexState inCombatComplexState, E_StateSO inStateSo, StateMachineBase inStateMachine) : base(inCombatComplexState.EnemyBrain, inStateSo, inStateMachine)
    {
        InCombatComplexState = inCombatComplexState;
    }
}