
using UnityEngine;

[CreateAssetMenu(menuName = "Nilsh/Inventory/Consumables/HealthItemData")]
public class HealthItemData : ObtainableItem
{
    [SerializeField] private int healthToAdd = 2;
    
    public override void ObtainItem(GameObject obtainedBy)
    {
        GameManager.Instance.playerData.EntityStats.ChangeHealth(healthToAdd);
    }
    
    public override EItemId GetItemId()
    {
        return EItemId.HealingItem;
    }
}
