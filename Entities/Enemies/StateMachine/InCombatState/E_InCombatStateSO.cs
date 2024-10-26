using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using VInspector;

[CreateAssetMenu(menuName = "Enemies/InCombat/MainCombatState")]
public class E_InCombatStateSO : E_StateSO
{
    public List<E_AttackCauseSO> attackCauses;
    [HideInInspector] public E_AttackCauseSO currentCause = null;

    [Tooltip("0 means it will be checked in its update method")] [Range(0, 10)]
    public float timeBetweenCausesQuery = 0.4f;

    public float timeWithoutSenseToReturnToIdle = 5f;

    [SerializeField] public E_AttackCauseSO[] attackCausesInstances;

    private Coroutine _queryCoroutine;

    [FormerlySerializedAs("attackStateSO")] [Header("Combat States")] [SerializeField]
    public E_AttackStateSO attackStateSo;

    [SerializeField] public E_CombatMovementStateSO combatMovementStateSO;

    [HideInInspector] public UnityEvent shouldAttack = new UnityEvent();

    public override void OnValidateSetup(EnemyBrain inBrain)
    {
        base.OnValidateSetup(inBrain);

        attackCausesInstances = new E_AttackCauseSO[attackCauses.Count];

        for (int i = 0; i < attackCauses.Count; ++i)
        {
            attackCausesInstances[i] = Instantiate(attackCauses[i]);
        }

        foreach (E_AttackCauseSO cause in attackCausesInstances)
        {
            cause.OnValidateSetup(inBrain);
        }
    }

    public override void Enter()
    {
        base.Enter();

        foreach (E_AttackCauseSO cause in attackCausesInstances)
        {
            cause.StartChecking();
        }

        CheckChangeToAttack();
    }

    public override void Exit()
    {
        foreach (E_AttackCauseSO cause in attackCausesInstances)
        {
            cause.EndChecking();
        }

        base.Exit();

        if (_queryCoroutine != null)
        {
            EnemyBrain.StopCoroutine(_queryCoroutine);
        }
    }

    public override void Update()
    {
        foreach (E_AttackCauseSO cause in attackCausesInstances)
        {
            cause.Update();
        }

        if (timeBetweenCausesQuery == 0)
        {
            CheckChangeToAttack();
        }
    }

    private IEnumerator QueryDelayCoroutine()
    {
        yield return new WaitForSeconds(timeBetweenCausesQuery);
        CheckChangeToAttack();
    }

    private void CheckChangeToAttack()
    {
        bool shouldChangeToAttack = DoQueryAttackCauses();

        if (timeBetweenCausesQuery > 0)
        {
            _queryCoroutine = EnemyBrain.StartCoroutine(QueryDelayCoroutine());
        }

        if (shouldChangeToAttack)
        {
            shouldAttack.Invoke();
        }
    }

    //Returns true if we should start an attack
    public bool DoQueryAttackCauses()
    {
        foreach (E_AttackCauseSO cause in attackCausesInstances)
        {
            if (cause.CanAttack())
            {
                currentCause = cause;
                return true;
            }
        }

        return false;
    }


    public override void OnHit(bool isDead)
    {
        //If it dies, we always want to change to hitState
        if (isDead)
        {
            base.OnHit(isDead);
        }
        else
        {
            //If the attackAction can be interrupted, we do so
            if (currentCause.attackActionInstance.bCanBeInterruptedByHit)
            {
                base.OnHit(isDead);
            }
        }
    }
}