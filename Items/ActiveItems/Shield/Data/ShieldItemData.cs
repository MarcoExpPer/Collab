using UnityEngine;

[CreateAssetMenu(menuName = "Nilsh/Inventory/Items/ShieldItemData")]
public class ShieldItemData : ActiveItemData
{
    public GameObject ShieldPrefab;
    
    public override void ObtainItem(GameObject ownerGameObject)
    {
        ownerGameObject.AddComponent<ActiveShield>().OnObtained(this);
    }
    
    public override EItemId GetItemId()
    {
        return EItemId.Shield;
    }
}