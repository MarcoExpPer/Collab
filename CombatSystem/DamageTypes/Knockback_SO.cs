
using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "CombatSystem/Knockback")]
public class Knockback_SO : HitEffect_SO
{
    [SerializeField] private int force = 25;
    public int Force => force;
    
    public void Awake()
    {
        DamageType = EDamageType.Knockback;
    }
}