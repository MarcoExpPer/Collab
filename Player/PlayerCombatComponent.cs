using System;
using System.Collections;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerCombatComponent : NewCombatComponent
{
    public string DeathAnimationState ="Death";
    
    private Animator animator;
    
    protected void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    protected override void Start()
    {
        base.Start();
        
        statsInstance.onHealthZero.AddListener(OnHealthZero);
        statsInstance.onHealthChanged.AddListener(OnHealthChanged);
    }
    
    public override void OnDestroy()
    {
        base.OnDestroy();
        
        statsInstance.onHealthZero.RemoveListener(OnHealthZero);
        statsInstance.onHealthChanged.RemoveListener(OnHealthChanged);
    }
    

    protected override EntityStatsData GetOrCreateEntityStatsInstance()
    {
        return GameManager.Instance.playerData.EntityStats;
    }

    protected override void OnInstantDamage(InstantDamage_SO iDamageSoSo)
    {
        base.OnInstantDamage(iDamageSoSo);
        
        if (iDamageSoSo.DoHitAnimation && statsInstance.health > 0)
        {
            //Do animation
        }
    }
    
    protected override void OnDoTDamage(Dot_SO iDotSO)
    {
        base.OnDoTDamage(iDotSO);
        
        if (iDotSO.DoHitAnimation && statsInstance.health > 0)
        {
            //Do animation
        }
    }

    private void OnHealthZero()
    {
        //Player Respawn
        Time.timeScale = 0;
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        GetComponent<Animator>().Play(DeathAnimationState); 
    }

    public void OnDeathAnimationFinished()
    {
        animator.updateMode = AnimatorUpdateMode.Normal;
        GameManager.Instance.uiManager.DeathScreen.OnPlayerDeath();
    }

    private void OnHealthChanged(int previousHealth, int currentHealth, int maxHealth)
    {
        if (previousHealth < currentHealth)
        {
            //Player healed
        }
        else
        {
            flashController.StartFlash();
        }
    }
}
