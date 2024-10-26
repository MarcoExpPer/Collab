using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Enemies/Idle/RandomIdle")]
public class E_RandomIdleStateSO : E_IdleStateSO
{
    [HideInInspector] [SerializeField] private NavMeshAgent enemyAgent;
    private Vector3 initialPosition;
    
    public float waitTime = 2;
    public float randomMovementRange = 5;
    public LayerMask obstacleLayerMask;
    
    private Coroutine _waitCoroutine;
    private bool _isWaiting = false;
    private NavMeshPath _currentPath = null;
    
    public override void Initialize(EnemyBrain inBrain)
    {
        base.Initialize(inBrain);
        enemyAgent = EnemyBrain.navMeshAgent;
        initialPosition = enemyAgent.transform.position;
    }

    public override void Enter()
    {
        base.Enter();
        
        enemyAgent.ResetPath();
        enemyAgent.isStopped = false;
        enemyAgent.updateRotation = true;
        MoveToRandomLocation();
    }
    
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (!_isWaiting && !enemyAgent.pathPending && !enemyAgent.hasPath)
        {
            _waitCoroutine = EnemyBrain.StartCoroutine(WaitAtTargetLocation());
        }
    }
    
    public override void Exit()
    {
        if (_waitCoroutine != null)
        {
            EnemyBrain.StopCoroutine(_waitCoroutine);  
        }
        
        enemyAgent.ResetPath();
        enemyAgent.updateRotation = false;
        enemyAgent.isStopped = true;
    }

    public override void OnHit(bool isDead)
    {
        //EnemyBase.StateMachine.ChangeState(EnemyBase.HitState);
    }

    private void MoveToRandomLocation()
    {
        _isWaiting = false;
        
        
        _currentPath = new NavMeshPath();
        Vector3 alternativeTargetPoint = GetRandomNavigablePosition(_currentPath);
        
        //Most of the time it should find a path, either by the normal way or the raycast way
        if (_currentPath.corners.Length > 0)
        {
            enemyAgent.SetPath(_currentPath);
        }
        else
        {
            enemyAgent.SetDestination(alternativeTargetPoint);
            _currentPath = enemyAgent.path;
        }
    }

    //Find a random navigable point around initialPosition
    private Vector3 GetRandomNavigablePosition(NavMeshPath targetPath)
    {
        Vector2 random2DPointInCircle = (Random.insideUnitCircle * randomMovementRange);
        Vector3 random3DPosition = new Vector3(random2DPointInCircle.x, 0, random2DPointInCircle.y) + initialPosition;

        //if the random point is navigable,return such point
        if (NavMesh.CalculatePath(enemyAgent.transform.position, random3DPosition, NavMesh.AllAreas, targetPath))
        {
            return targetPath.corners[^1];
        }
        
        //If its not navigable, try to recuperate from with a raycast and moving to the raycast hit position
        Vector3 direction = (random3DPosition - enemyAgent.transform.position).normalized;
        if (Physics.Raycast(enemyAgent.transform.position, direction, out RaycastHit hitInfo, randomMovementRange,
                obstacleLayerMask))
        {   
            //if we dont reduce the distance, the target point will be the wall that we hit with the raycast
            random3DPosition = hitInfo.point - direction * (enemyAgent.radius );
            
            //Check if the new point is navigable (usually it should always be, altought it may be too close to the current location of the target
            if (NavMesh.CalculatePath(enemyAgent.transform.position, random3DPosition, NavMesh.AllAreas,
                    targetPath))
            {
                return random3DPosition;
            }
        }
        return initialPosition;
    }

    private IEnumerator WaitAtTargetLocation()
    {
        _isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        MoveToRandomLocation();
    }
}
