using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyBrain))]
public class EnemyAnimationController : MonoBehaviour
{
    [Header("State parameters (bool")] public string isMoving = "isMoving";
    private bool _cachedIsMoving = false;

    [Header("State parameters (int")]
    //0 == Idle, 1 == Alert, 2 == InCombat, 3 == ReturnToidle
    public string state = "State";

    //Cached components
    [SerializeField,HideInInspector] private EnemyBrain enemyBrain;
    [SerializeField,HideInInspector] private NavMeshAgent navMeshAgent;
    [SerializeField,HideInInspector] private Animator animator;
    
    private void OnValidate()
    {
        enemyBrain = GetComponent<EnemyBrain>();
        navMeshAgent = gameObject.GetComponentInParent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (!navMeshAgent)
        {
            Debug.LogWarning("No navmesh agent detected in EnemyAnimationController");
        }

        if (!enemyBrain)
        {
            Debug.LogWarning("No enemyBrain detected in EnemyAnimationController");
        }
    }

    private void Awake()
    {
        enemyBrain.onActivationTiggered.AddListener(OnActivationTiggered);
        enabled = enemyBrain.enabled;
    }

    private void OnActivationTiggered(bool isActivated)
    {
        enabled = isActivated;
    }

    private void OnEnable()
    {
        animator.enabled = true;
        enemyBrain.StateMachine.onStateChanged.AddListener(OnStateChanged);
    }
    
    private void OnDisable()
    {
        animator.enabled = false;
        if (enemyBrain.StateMachine != null)
        {
            enemyBrain.StateMachine.onStateChanged.RemoveListener(OnStateChanged);
        }
    }

    private void Update()
    {
        //Only set the animator parameter when it changes, not constantly in the update if its the same parameter
        if (navMeshAgent.velocity.sqrMagnitude > 0.1 && !_cachedIsMoving)
        {
            _cachedIsMoving = true;
            animator.SetBool(isMoving, _cachedIsMoving);
        }
        else if (navMeshAgent.velocity.sqrMagnitude < 0.1 && _cachedIsMoving)
        {
            _cachedIsMoving = false;
            animator.SetBool(isMoving, _cachedIsMoving);
        }
    }
    
    private void OnStateChanged(StateBase previous, StateBase current)
    {
        if (current == enemyBrain.IdleState)
        {
            animator.SetInteger(state, 0);
        }
        else if (current == enemyBrain.AlertState)
        {
            animator.SetInteger(state, 1);
        }
        else if (current == enemyBrain.InCombatComplexState)
        {
            animator.SetInteger(state, 2);
        }
        else if (current == enemyBrain.ReturnToIdleState)
        {
            animator.SetInteger(state, 3);
        }
        //We intentionaly not check for HIT state because its not related to the state animation parameter
    }
    
    public enum EAttackAnimationEvents
    {
        DoAttackEffects,
        AttackAnimationFinished
    }

    public void AttackAnimationEvent(EAttackAnimationEvents eventTag)
    {
        switch (eventTag)
        {
            case EAttackAnimationEvents.DoAttackEffects:
                enemyBrain.InCombatComplexState.InCombatStateSO.currentCause.attackActionInstance.DoAttackEffects();
                break;
            case EAttackAnimationEvents.AttackAnimationFinished:
                enemyBrain.InCombatComplexState.InCombatStateSO.currentCause.attackActionInstance.FinishAttack();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(eventTag), eventTag, null);
        }
    }
    
    public enum EOtherAnimationEvents
    {
        HitFinished,
        DeadFinished
    }
    
    public void OtherAnimationEvents(EOtherAnimationEvents eventTag)
    {
        switch (eventTag)
        {
            case EOtherAnimationEvents.HitFinished:
                enemyBrain.HitState.hitStateSO.OnHitFinished();
                break;
            case EOtherAnimationEvents.DeadFinished:
                enemyBrain.HitState.hitStateSO.OnDeathFinished();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(eventTag), eventTag, null);
        }
    }
}