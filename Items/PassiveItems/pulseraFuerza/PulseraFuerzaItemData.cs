using UnityEngine;


[CreateAssetMenu(menuName = "Nilsh/Inventory/Items/PulseraFuerzaItemData")]
public class PulseraFuerzaItemData : PassiveItemData
{
    public override EItemId GetItemId()
    {
        return EItemId.PulseraFuerza;
    }
}


