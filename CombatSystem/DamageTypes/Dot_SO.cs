using UnityEngine;

[CreateAssetMenu(menuName = "CombatSystem/Dot")]
public class Dot_SO : HitEffect_SO
{
    [SerializeField] private int damagePerTick = 1;
    [SerializeField] private int totalTicks = 4;
    [SerializeField] private float period = 0.5f;

    // Public read-only properties
    public int DamagePerTick => damagePerTick;
    public int TotalTicks => totalTicks;
    public float Period => period;
    
    public void OnValidate()
    {
        DamageType = EDamageType.DoT;
    }
}
