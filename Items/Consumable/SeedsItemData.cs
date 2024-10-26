
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Nilsh/Inventory/Consumables/SeedsItemData")]
public class SeedsItemData : ObtainableItem
{
    [SerializeField] private int seedsToAdd = 20;
    
    public override void ObtainItem(GameObject obtainedBy)
    {
        GameManager.Instance.playerData.ChangeSeeds(seedsToAdd);
    }
    
    public override EItemId GetItemId()
    {
        return EItemId.Seeds;
    }
}
