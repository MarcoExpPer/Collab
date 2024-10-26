
using UnityEngine;

public enum EDamageType
{
    InstantDamage = 0,
    DoT,
    Slow,
    Stun,
    Knockback,
    MAX,
    INVALID
}

public enum EDamageElement
{
    Generic = 0,
    Fire,
    Water,
    DeadZone,
    MAX
}


public abstract class HitEffect_SO : ScriptableObject
{
    [SerializeField] private bool doHitAnimation = false;
    [SerializeField] private bool doHitVFX = false;
    
    public bool DoHitAnimation => doHitAnimation;
    public bool DoHitVFX => doHitVFX;
    
    public EDamageType DamageType { get; protected set; } = EDamageType.INVALID;
    public EDamageElement DamageElement { get; protected set; } = EDamageElement.Generic;
}
    
