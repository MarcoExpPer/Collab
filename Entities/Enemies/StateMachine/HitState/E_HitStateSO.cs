
using UnityEngine;
using UnityEngine.Serialization;
using VInspector;

[CreateAssetMenu(menuName = "Enemies/Hit/SimpleHit")]
public class E_HitStateSO: E_StateSO
{
    public bool hasAnimations = false;
    [ShowIf("hasAnimations")]
    public string hitAnimationStatename = "Hit";
    public string deadAnimationStateName = "Dead";
    public bool allowEffectsRetriggerWhileExecuting = true;
    [EndIf]
    
    [HideInInspector] public bool isDead;
    [SerializeField, HideInInspector] Animator animator;
    [SerializeField, HideInInspector] NavAgentKnockbackController knockback;
    private bool waitingToExit = false;
    
    public override void OnValidateSetup(EnemyBrain inBrain)
    {
        base.OnValidateSetup(inBrain);

        if (hasAnimations)
        {
            animator = inBrain.GetComponent<Animator>(); 
        }
        
        knockback = inBrain.GetComponentInParent<NavAgentKnockbackController>();
    }


    public override void Enter()
    {
        base.Enter();

        if (isDead)
        {
            DoDeathEffects();
        }
        else
        {
            DoHitEffects(); 
        }

        waitingToExit = false;
        knockback.onKnockbackFinished.AddListener(OnKnockbackFinished);
    }

    public override void Exit()
    {
        base.Exit();
        
        knockback.onKnockbackFinished.RemoveListener(OnKnockbackFinished);
    }

    private void OnKnockbackFinished()
    {
        if (waitingToExit)
        {
            EnemyBrain.StateMachine.ChangeState(EnemyBrain.InCombatComplexState); 
        }
    }

    protected virtual void DoHitEffects()
    {
        if (hasAnimations)
        {
            animator.Play(hitAnimationStatename, -1, 0);
        }
        else
        {
            OnHitFinished();
        }
    }

    protected virtual void DoDeathEffects()
    {
        if (hasAnimations)
        {
            animator.Play(deadAnimationStateName);
        }
        else
        {
            OnDeathFinished();
        }
    }

    public virtual void OnHitFinished()
    {
        //When a hit animation ends, the enemy may still be stuned or doing knockback
        if (EnemyBrain.navMeshAgent.enabled)
        {
            EnemyBrain.StateMachine.ChangeState(EnemyBrain.InCombatComplexState); 
        }
        else
        {
            waitingToExit = true;
        }
    }

    public virtual void OnDeathFinished()
    {
        Destroy(EnemyBrain.gameObject);
    }
    
    public override void OnHit(bool isDead)
    {
        //We dont call super because we dont want to change to this same state, we already have access to the hit effects here
        if (isDead)
        {
            DoDeathEffects();
        }
        else
        if (allowEffectsRetriggerWhileExecuting)
        {
            DoHitEffects();
        }
    }
}
