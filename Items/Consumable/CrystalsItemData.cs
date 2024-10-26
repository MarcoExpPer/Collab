
using UnityEngine;

[CreateAssetMenu(menuName = "Nilsh/Inventory/Consumables/CrystalsItemData")]
public class CrystalsItemData : ObtainableItem
{
    [SerializeField] private int crystalsToAdd = 1;
    
    public override void ObtainItem(GameObject obtainedBy)
    {
        GameManager.Instance.playerData.ChangeCrystasls(crystalsToAdd);
    }
    
    public override EItemId GetItemId()
    {
        return EItemId.Crystals;
    }
}
