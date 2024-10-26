
using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class NewCombatComponent : MonoBehaviour, INewHitable
{
    [SerializeField, HideInInspector] public bool[] elementalImmunities = new bool[(int) EDamageElement.MAX];
    [SerializeField, HideInInspector] public bool[] typeImmunities = new bool[(int) EDamageType.MAX];
    
    //This variables are only to ease the setup from editor. In runtime the arrays should be used to access and to edit
    //the inmunities
    [Header("Type Inmunities")] 
    [SerializeField] private bool instantDamage = false;
    [SerializeField] private bool dotDamage = false;
    [SerializeField] private bool slow = false;
    [SerializeField] private bool stun = false;
    [SerializeField] private bool knockback = false;
    [Header("Elemental Inmunities")]
    [SerializeField] private bool generic = false;
    [SerializeField] private bool fire = false;
    [SerializeField] private bool water = false;

    private const bool DeadZone = false; //Cant be inmune to DeadZone type of damage
    
    /* This parameter is the Instance of the EntityStats, so it should be the one used in all circunstances*/
    [HideInInspector] public EntityStatsData statsInstance;
    
    // List of active coroutines in case we need to remove them
    private readonly List<Coroutine> dotCoroutines = new();
    private int amountOfDots = 0;
    
    //Cached
    [SerializeField, HideInInspector] protected FlashController flashController;
    [SerializeField, HideInInspector] protected IKnockbackable KnockbackComponent;
    protected virtual void Start()
    {
        flashController = GetComponent<FlashController>();
        KnockbackComponent = GetComponent<IKnockbackable>();
        
        statsInstance = GetOrCreateEntityStatsInstance();
        statsInstance.ChangeHealth(statsInstance.maxHealth);

        if (KnockbackComponent == null)
        {
            typeImmunities[(int)EDamageType.Knockback] = false;
        }
    }
    
    public bool TryHit(HitEffect_SO hitEffectSo, GameObject hitSource)
    {   
        if (IsInmune(hitEffectSo.DamageElement, hitEffectSo.DamageType))
        {
            return false;
        }
        
        if (statsInstance.health <= 0)
        {
            return false;
        }
        
        if (hitEffectSo.DoHitVFX)
        {
            flashController.StartFlash();
        }

        switch (hitEffectSo.DamageType)
        {
            case EDamageType.InstantDamage:
            {
                OnInstantDamage((InstantDamage_SO)hitEffectSo);
                break;
            }
            case EDamageType.DoT:
            {
                OnApplyDotDamage((Dot_SO)hitEffectSo);
                break;
            }
            case EDamageType.Knockback:
            {
                OnKnockback((Knockback_SO)hitEffectSo, hitSource.transform.position);
                break;
            }
    }


        return true;
    }
#region InstantDamage
    protected virtual void OnInstantDamage(InstantDamage_SO iDamageSoSo)
    {
        statsInstance.ChangeHealth(-iDamageSoSo.Damage);
    }
#endregion InstantDamage
#region DOTS

    protected virtual void OnApplyDotDamage(Dot_SO dotSoDamageSo)
    {
        dotCoroutines.Add(StartCoroutine(DotCoroutine(dotSoDamageSo)));
    }

    protected virtual void OnDoTDamage(Dot_SO dotSoDamageSo)
    {
        statsInstance.ChangeHealth(-dotSoDamageSo.DamagePerTick);
    }
    

    private IEnumerator DotCoroutine(Dot_SO dotSoDamageSo)
    {
        amountOfDots++;
        OnDoTDamage(dotSoDamageSo);
        for (int ticks = 1; ticks < dotSoDamageSo.TotalTicks; ticks++)
        {
            yield return new WaitForSeconds(dotSoDamageSo.Period);
            OnDoTDamage(dotSoDamageSo);
        }

        amountOfDots--;
        if (amountOfDots == 0)
        {
            dotCoroutines.Clear();
        }
    }

    public void StopAllDots()
    {
        foreach (Coroutine coroutine in dotCoroutines)
        {
            StopCoroutine(coroutine);
        }
        
        dotCoroutines.Clear();
    }
    
#endregion DOTS
#region Knockback
protected virtual void OnKnockback(Knockback_SO iKnockbackSo, Vector3 sourcePosition)
{
    //Remove any vertical knockback
    sourcePosition.y = transform.position.y;
    Vector3 direction = (transform.position - sourcePosition).normalized;
    
    KnockbackComponent.ApplyImpulse(direction, iKnockbackSo.Force);
}
#endregion Knockback

    public virtual void OnDestroy()
    {
        StopAllDots();
    }

    /*
     * There are 4 cases that can happen
     * The player is not inmune to anything -> returns false
     * The player is inmune to the element but not the type -> returns true
     * The player is inmune to both -> returns true
     * The player is inmune to the type but the element is Deadzone -> returns false because deadzone is never inmune
     */
    private bool IsInmune(EDamageElement element, EDamageType type)
    {
        bool typeResult = typeImmunities[(int)type];
        bool elementResult = elementalImmunities[(int)element];
        
        return typeResult && (elementResult && element != EDamageElement.DeadZone);
    }
    
    public virtual void OnValidate()
    {
        elementalImmunities = new bool[(int) EDamageElement.MAX];
        typeImmunities = new bool[(int) EDamageType.MAX];
        
        elementalImmunities[(int) EDamageElement.Generic] = generic;
        elementalImmunities[(int) EDamageElement.Fire] = fire;
        elementalImmunities[(int) EDamageElement.Water] = water;
        elementalImmunities[(int) EDamageElement.DeadZone] = DeadZone;
        
        typeImmunities[(int) EDamageType.InstantDamage] = instantDamage;
        typeImmunities[(int) EDamageType.DoT] = dotDamage;
        typeImmunities[(int) EDamageType.Slow] = slow;
        typeImmunities[(int) EDamageType.Stun] = stun;
        typeImmunities[(int) EDamageType.Knockback] = knockback;
    }
    
    protected virtual EntityStatsData GetOrCreateEntityStatsInstance()
    {
        Debug.LogError("GetOrCreateEntityStatsInstance MUST be overriden");
        return null;
    }
}