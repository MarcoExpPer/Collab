using UnityEngine;
using UnityEngine.Serialization;

public abstract class E_AttackCauseSO : ScriptableObject
{
    [SerializeField] public E_AttackActionSO attackActionInstance;
    public virtual void OnValidateSetup(EnemyBrain enemyBrain)
    {
        attackActionInstance = Instantiate(attackActionInstance);
        attackActionInstance.OnValidateSetup(enemyBrain);
    }
    
    public virtual void OnRuntimeSetup(EnemyBrain enemyBrain)
    {
        attackActionInstance.OnRuntimeSetup(enemyBrain);
    }
    
    //On InCombatState Enter
    public virtual void StartChecking()
    {
    }
    
    //On InCombatState Exit
    public virtual void EndChecking()
    {
    }
    
    //On InCombatState Update
    public virtual void Update()
    {
    }
    
    //On InCombatState Update
    public virtual bool CanAttack()
    {
        return false;
    }

    public virtual void StartAttack()
    {
        attackActionInstance.StartAttack();
    }
}