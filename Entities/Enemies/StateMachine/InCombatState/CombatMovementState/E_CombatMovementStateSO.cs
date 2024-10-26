
using UnityEngine;


[CreateAssetMenu(menuName = "Enemies/InCombat/Movement/NoMovement")]
public class E_CombatMovementStateSO : E_StateSO
{

    public bool lookAtPlayer = true;


    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (lookAtPlayer)
        {
            Vector3 playerLocation = GameManager.Instance.playerSmController.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(playerLocation - EnemyBrain.transform.position, Vector3.up );
            EnemyBrain.transform.rotation = targetRotation;
        }
    }
}