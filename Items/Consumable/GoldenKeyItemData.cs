
using UnityEngine;

[CreateAssetMenu(menuName = "Nilsh/Inventory/Consumables/GoldenKeyItemData")]
public class GoldenKeyItemData : ObtainableItem
{
    [SerializeField] private int keysToAdd = 1;
    
    public override void ObtainItem(GameObject obtainedBy)
    {
        GameManager.Instance.playerData.ChangeKeys(keysToAdd);
    }
    
    public override EItemId GetItemId()
    {
        return EItemId.GoldenKeys;
    }
}
