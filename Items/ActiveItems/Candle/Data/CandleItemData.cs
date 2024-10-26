using UnityEngine;

[CreateAssetMenu(menuName = "Nilsh/Inventory/Items/CandleItemData")]
public class CandleItemData : ActiveItemData
{
    public GameObject CandlePrefab;
    
    public override void ObtainItem(GameObject ownerGameObject)
    {
        ownerGameObject.AddComponent<ActiveCandle>().OnObtained(this);
    }
    
    public override EItemId GetItemId()
    {
        return EItemId.Candle;
    }
}