using UnityEngine;

[CreateAssetMenu(menuName = "Nilsh/Inventory/Items/BowItemData")]
public class BowItemData : ActiveItemData
{
    public int damage;
    public GameObject bowPrefab;
    
    public override void ObtainItem(GameObject ownerGameObject)
    {
        ownerGameObject.AddComponent<ActiveBow>().OnObtained(this);
    }
    
    public override EItemId GetItemId()
    {
        return EItemId.Bow;
    }
}