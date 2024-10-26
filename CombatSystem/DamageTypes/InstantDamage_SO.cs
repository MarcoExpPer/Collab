
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "CombatSystem/Instant Damage")]
public class InstantDamage_SO : HitEffect_SO
{
    [SerializeField] private int damage = 1;
    
    public int Damage => damage;

    
    public void Awake()
    {
        DamageType = EDamageType.InstantDamage;
    }
}