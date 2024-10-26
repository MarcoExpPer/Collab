
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Enemies/InCombat/Movement/ChasePlayer")]
public class E_ChasePlayerCombatMovementStateSO : E_CombatMovementStateSO
{
    NavMeshAgent agent;
    [SerializeField] private Transform playerTransform;

    Coroutine chasingCoroutine;

    public override void Initialize(EnemyBrain inBrain)
    {
        base.Initialize(inBrain);

        agent = inBrain.navMeshAgent;
        playerTransform = GameManager.Instance.playerSmController.transform;
    }

    public override void Enter()
    {
        base.Enter();

        agent.isStopped = false;
        agent.updateRotation = lookAtPlayer;
        MoveToPlayer();

        chasingCoroutine = EnemyBrain.StartCoroutine(DelayChase());
    }

    public override void Exit()
    {
        if (chasingCoroutine != null)
        {
            EnemyBrain.StopCoroutine(chasingCoroutine);
        }
        
        agent.isStopped = true;
        agent.updateRotation = false;
        agent.path = new NavMeshPath();
    }

    private void MoveToPlayer()
    {
        agent.SetDestination(playerTransform.position);
    }
    
    //Dont call base class to not autoupdate rotation, in this class the NavMeshAgent is the one rotation the enemy
    public override void FixedUpdate()
    {
        
    }

    private IEnumerator DelayChase()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);
            MoveToPlayer();
        }
    }
}
