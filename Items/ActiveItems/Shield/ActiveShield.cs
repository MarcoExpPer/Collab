

using System.Linq;
using UnityEngine;

public class ActiveShield : ActiveItem
{
    private ShieldItemData _shieldData;
    private GameObject shield;
    
    public override void OnObtained( ActiveItemData itemData )
    {
        CurrentRigSlot = ERigItemSlot.ShieldHand;
        currentEquipedItemSlot = EEquipedItemSlot.Shield;
        
        _shieldData = (ShieldItemData) itemData;
        
        base.OnObtained( itemData );
        Item.EquipItem(_shieldData.GetItemId(), EEquipedItemSlot.Shield);
        
        shield = FindPlayerSlotAndInstantiatePrefab(GameManager.PlayerShieldHandTag, _shieldData.ShieldPrefab);
        shield.SetActive(false);
    }

    public override bool OnInput(bool inputValue)
    {
        if (!base.OnInput(inputValue)) return false;

        if (inputValue)
        {
            if (!shield.activeSelf)
            {
                Item.onRigSlotUpdated.Invoke( CurrentRigSlot , this);
                shield.SetActive(true);
            } 
            
            //TODO Shield behaviour
        }

        return true;
    }


}
