
using System;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyCombatComponent : NewCombatComponent
{
    [HideInInspector, SerializeField] public EnemyBrain brain;
    
    [SerializeField] public EntityStatsData stats;
    
    protected override void Start()
    {
        base.Start();
        
        statsInstance.onHealthZero.AddListener(OnHealthZero);
    }
    
    public override void OnDestroy()
    {
        base.OnDestroy();
        
        statsInstance.onHealthZero.RemoveListener(OnHealthZero);
    }
    
    public override void OnValidate()
    {
        base.OnValidate();
        brain = transform.GetComponentInChildren<EnemyBrain>();
    }
    
    protected override EntityStatsData GetOrCreateEntityStatsInstance()
    {
        return Instantiate(stats);
    }

    private void OnHealthZero()
    {
        flashController.ForceStopFlash();
        brain.EnemyGotHit(true);
    }
    
    protected override void OnInstantDamage(InstantDamage_SO iDamageSoSo)
    {
        base.OnInstantDamage(iDamageSoSo);

        if (iDamageSoSo.DoHitAnimation && statsInstance.health > 0)
        {
            brain.EnemyGotHit(false);
        }
    }
    
    protected override void OnDoTDamage(Dot_SO dotSoDamageSo)
    {
        statsInstance.ChangeHealth(-dotSoDamageSo.DamagePerTick);
        
        if (dotSoDamageSo.DoHitAnimation && statsInstance.health > 0)
        {
            brain.EnemyGotHit(false);
        }
    }

    protected override void OnKnockback(Knockback_SO iKnockbackSo, Vector3 sourcePosition)
    {
        base.OnKnockback(iKnockbackSo, sourcePosition);
        
        if (iKnockbackSo.DoHitAnimation)
        {
            brain.EnemyGotHit(false); 
        }
    }
}
