using UnityEngine;


[CreateAssetMenu(menuName = "Nilsh/Inventory/Items/KeyDoorItemData")]
public class KeyDoorItemData : PassiveItemData
{
    public override EItemId GetItemId()
    {
        return EItemId.KeyDoor;
    }
}


