
    public class ECombatMovementStateBase : E_CombatStateBase
    {
        public ECombatMovementStateBase(E_InCombatComplexState inCombatComplexState, E_StateSO inStateSo, StateMachineBase inStateMachine) : base(inCombatComplexState, inStateSo, inStateMachine)
        {
            
        }

        public override void Enter()
        {
            base.Enter();

            if (InCombatComplexState.ManualAttackCausesQuery())
            {
                InCombatComplexState.OnShouldAttackEvent();
            }
        }
    }
