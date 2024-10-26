using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using VInspector;

[CreateAssetMenu(menuName = "Enemies/InCombat/AttackCauses/ByTrigger")]
public class E_AttackByTriggerSO : E_AttackCauseSO
{
    public bool doRaycast = true;
    [ShowIf("doRaycast")]
    [SerializeField] LayerMask _raycastObstacleMask;
    
    private bool _isPlayerInside;

    private const string AttackCauseTriggerGameObjectName = "attackCauseTrigger";
    
    [SerializeField] private Utils.ColliderSpawnInformation colliderSpawnInformation;
    [SerializeField, HideInInspector] private PlayerTrigger _attackTrigger;
    [SerializeField, HideInInspector] private EnemyBrain _enemyBrain; 
    public override void OnValidateSetup(EnemyBrain enemyBrain)
    {
        base.OnValidateSetup(enemyBrain);

        _enemyBrain = enemyBrain;
        colliderSpawnInformation.colliderGameObjectName = AttackCauseTriggerGameObjectName;
        _attackTrigger = Utils.FindOrCreatePlayerTriggerToGameObject(enemyBrain.gameObject, colliderSpawnInformation);
        
        _attackTrigger.enabled = false;
    }

    public override void StartChecking()
    {
        _attackTrigger.enabled = true;
    }

    public override void EndChecking()
    {
        _attackTrigger.enabled = false;
    }

    public override bool CanAttack()
    {
        if (!_attackTrigger.IsPlayerInside) return false;
        
        bool playerRaycasted = true;
        if (doRaycast)
        {
            Vector3 playerPos = GameManager.Instance.playerSmController.gameObject.transform.position;
            Vector3 distToPlayer = (playerPos - _enemyBrain.transform.position);
            Vector3 dirToPlayer = distToPlayer.normalized;
                
            if (Physics.Raycast (_enemyBrain.transform.position, dirToPlayer, distToPlayer.magnitude, _raycastObstacleMask.value ))
            {
                playerRaycasted = false;
            }
        }
        
        return playerRaycasted;
    }
}