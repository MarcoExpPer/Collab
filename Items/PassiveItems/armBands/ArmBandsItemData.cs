using UnityEngine;


//This passive item is just a dummy
[CreateAssetMenu(menuName = "Nilsh/Inventory/Items/ArmBandsItemData")]
public class ArmBandsItemData : PassiveItemData
{
    public override EItemId GetItemId()
    {
        return EItemId.ArmBands;
    }
}


