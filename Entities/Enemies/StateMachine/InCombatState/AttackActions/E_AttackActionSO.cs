using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public abstract class E_AttackActionSO : ScriptableObject
{
    [HideInInspector] [SerializeField] protected EnemyBrain enemyBrain;
    [HideInInspector] [SerializeField] public UnityEvent onAttackFinised = new UnityEvent();
    public bool bCanBeInterruptedByHit;
    public float afterAttackDelay = 1f;
    public bool lookAtPlayerOnAttack = true;
    
    public bool hasAnimation = false;
    //[ShowIf("hasAnimation")]
    [SerializeField] public string attackAnimationStateName = "Attack";

    
    private Coroutine _delayAfterAttackCoroutine;

    [HideInInspector] [SerializeField] protected Animator animator;
    public virtual void OnValidateSetup(EnemyBrain enemyBrain)
    {
        this.enemyBrain = enemyBrain;

        animator = hasAnimation ? enemyBrain.GetComponent<Animator>() : null;
    }

    public virtual void OnRuntimeSetup(EnemyBrain enemyBrain)
    {
    }
    
    //Intended to be when the attack animation starts and w/e needs to be do when the enemy is starting the attack process
    public virtual void StartAttack()
    {
        if (lookAtPlayerOnAttack)
        {
            Vector3 playerLocation = GameManager.Instance.playerSmController.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(playerLocation - enemyBrain.transform.parent.position, Vector3.up );
            enemyBrain.transform.parent.rotation = targetRotation;
        }

        if (hasAnimation)
        {
            animator.Play(attackAnimationStateName);
        }
    }
    
    //Intended to be when the enemy will actually apply the effects of the attack
    public virtual void DoAttackEffects()
    {
        
    }

    //This may need to be used to interrupt an attack if necesary
    public virtual void OnAttackStateExit()
    {
        if (_delayAfterAttackCoroutine != null)
        {
            enemyBrain.StopCoroutine(_delayAfterAttackCoroutine);
        }
    }
    
    //Called when an attack animation has finished
    public void FinishAttack()
    {
        if (afterAttackDelay > 0)
        {
            _delayAfterAttackCoroutine = enemyBrain.StartCoroutine(AfterAttackDelayCoroutine());
        }
        else
        {
            onAttackFinised.Invoke();
        }
    }

    private IEnumerator AfterAttackDelayCoroutine()
    {
        yield return new WaitForSeconds(afterAttackDelay);
        onAttackFinised.Invoke();
    }
}