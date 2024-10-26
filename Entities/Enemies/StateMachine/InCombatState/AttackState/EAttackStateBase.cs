
    using UnityEngine;

    public class EAttackStateBase : E_CombatStateBase
    {
        public EAttackStateBase(E_InCombatComplexState inCombatComplexState, E_AttackStateSO inStateSo, StateMachineBase inStateMachine) : base(inCombatComplexState, inStateSo, inStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            InCombatComplexState.InCombatStateSO.currentCause.attackActionInstance.onAttackFinised.AddListener(OnAttackFinished);
            InCombatComplexState.InCombatStateSO.currentCause.StartAttack();
        }

        public override void Exit()
        {
            InCombatComplexState.InCombatStateSO.currentCause.attackActionInstance.onAttackFinised.RemoveListener(OnAttackFinished);
            InCombatComplexState.InCombatStateSO.currentCause.attackActionInstance.OnAttackStateExit();
        }

        private void OnAttackFinished()
        {
            InCombatComplexState.InCombatStateMachine.ChangeState(InCombatComplexState.CombatMovementStateBase);
        }
    }
