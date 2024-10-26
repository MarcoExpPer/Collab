using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/InCombat/AttackCauses/ByCooldown")]
public class E_AttackByCooldownSO : E_AttackCauseSO
{
    public float cooldownTime = 5f;
    public bool startReady = false;
    [Tooltip("If true, while the player is doing another attack this cooldown will keep ticking")]
    public bool allowCooldownToProgressWhileAttacking = false;

    private float _currentTime = 0f;

    private bool _attackReady = false;
    [SerializeField] private EnemyBrain _enemyBrain;
    
    public override void OnValidateSetup(EnemyBrain enemyBrain)
    {
        base.OnValidateSetup(enemyBrain);
        
        _enemyBrain = enemyBrain;
    }
    public override void OnRuntimeSetup(EnemyBrain enemyBrain)
    {
        base.OnRuntimeSetup(enemyBrain);
        
        _currentTime = 0;
    }

    public override void StartChecking()
    {
        base.StartChecking();
        
        if (startReady)
        {
            _attackReady = true;
        }
    }
    
    public override void Update()
    {
        base.Update();
        if (_attackReady || !CanProgressCooldown()) return;
        
        _currentTime += Time.deltaTime;
        if (_currentTime >= cooldownTime)
        {
            _attackReady = true;
        }
    }

    private bool CanProgressCooldown()
    {
        return allowCooldownToProgressWhileAttacking || (_enemyBrain.InCombatComplexState.InCombatStateMachine.CurrentState !=
                                                         _enemyBrain.InCombatComplexState.AttackStateBase);
    }
    public override bool CanAttack()
    {
        return _attackReady;
    }

    public override void StartAttack()
    {
        _currentTime -= cooldownTime;
        _attackReady = false;
        
        base.StartAttack();
    }
}