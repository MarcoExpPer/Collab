
using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/ReturnToIdle/Walk")]
public class E_ReturnToIdleStateSO : E_StateSO
{
    public float ignoreSensesTime = 1f;
    [HideInInspector] [SerializeField] public PlayerTrigger playerTrigger;
    [SerializeField] private Utils.ColliderSpawnInformation colliderSpawnInformation;

    private const string ReturnToIdleTriggername = "ReturnToIdleTrigger";

    public override void OnValidateSetup(EnemyBrain inBrain)
    {
        colliderSpawnInformation.isSphereCollider = true;

        colliderSpawnInformation.colliderGameObjectName = ReturnToIdleTriggername;// + inBrain.GetInstanceID();
        playerTrigger = Utils.FindOrCreatePlayerTriggerToGameObject(inBrain.gameObject, colliderSpawnInformation);
        playerTrigger.onPlayerExitTrigger.RemoveAllListeners();
        playerTrigger.onPlayerExitTrigger.AddListener(OnPlayerExitTrigger);
        
        playerTrigger.enabled = false;
    }

    public override void Initialize(EnemyBrain inBrain)
    {
        base.Initialize(inBrain);
        playerTrigger.transform.SetParent(null);
    }

    private void OnPlayerExitTrigger(GameObject playerGameObject)
    {
        if (EnemyBrain.StateMachine.CurrentState == EnemyBrain.InCombatComplexState)
        {
            EnemyBrain.StateMachine.ChangeState(EnemyBrain.ReturnToIdleState);
        }
    }
}
