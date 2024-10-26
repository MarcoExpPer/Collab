
using UnityEngine;

[CreateAssetMenu(menuName = "Nilsh/Inventory/Consumables/BombAmmoItemData")]
public class BombAmmoItemData : ObtainableItem
{
    [SerializeField] private int bombsToAdd = 10;
    
    public override void ObtainItem(GameObject obtainedBy)
    {
        
        //Idealy we should not be able to loot bombs if we dont have the bomb item, but who knows 
        ActiveBombs ActiveBombs = obtainedBy.GetComponent<ActiveBombs>();
        if (ActiveBombs)
        {
            ActiveBombs.ChangeAmmo(bombsToAdd);
        }
    }
    
    public override EItemId GetItemId()
    {
        return EItemId.BombsAmmo;
    }
}
