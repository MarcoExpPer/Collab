

using System.Linq;
using UnityEngine;

public class ActiveCandle : ActiveItem
{
    private CandleItemData _candleData;
    
    public override void OnObtained( ActiveItemData itemData )
    {
        _candleData = (CandleItemData) itemData;
        CurrentRigSlot = ERigItemSlot.ShieldHand;
        
        base.OnObtained( itemData );
       
        FindPlayerSlotAndInstantiatePrefab(GameManager.PlayerShieldHandTag, _candleData.CandlePrefab);
        RigGameObject.SetActive(false);
    }

    public override bool OnInput(bool inputValue)
    {
        if (!base.OnInput(inputValue)) return false;

        if (inputValue)
        {
            RigGameObject.SetActive(!RigGameObject.activeSelf);
            if (RigGameObject.activeSelf)
            {
                Item.onRigSlotUpdated.Invoke( CurrentRigSlot , this);
            }
        }
        
        return true;
    }
}
