
    using UnityEngine;
    using UnityEngine.Events;

    public class E_HitState : E_State
    {
        //false if its a hit, true if its dead
        
        public E_HitStateSO hitStateSO;
        public bool isDead = false;
        public E_HitState(EnemyBrain inBrain, E_HitStateSO inStateSo, StateMachineBase inStateMachine) : base(inBrain, inStateSo, inStateMachine)
        {
            hitStateSO = inStateSo;
            
        }

        public override void Enter()
        {
            hitStateSO.isDead = isDead;
            
            base.Enter();
        }


        private void PlayHitEffects()
        {
            
        }
    }
