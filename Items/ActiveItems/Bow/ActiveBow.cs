
using UnityEngine;

public class ActiveBow : ActiveItem
{
    private BowItemData _bowData;
    private GameObject _bow;
    
    public override void OnObtained( ActiveItemData itemData )
    {
        CurrentRigSlot = ERigItemSlot.BothHands;
        _bowData = (BowItemData) itemData;
        
        base.OnObtained( itemData );
        
        //Setup bow specific thingies
        FindPlayerSlotAndInstantiatePrefab(GameManager.PlayerShieldHandTag, _bowData.bowPrefab);
        RigGameObject.SetActive(false);
    }

    public override bool OnInput(bool inputValue)
    {
        if (!base.OnInput(inputValue)) return false;

        if (inputValue)
        {
            if (!RigGameObject.activeSelf)
            {
                Item.onRigSlotUpdated.Invoke( CurrentRigSlot , this);
                RigGameObject.SetActive(true);
            }
        
            //Spawn Arrow or animation or something
            ChangeAmmo(-1);
            print("SPAWN ARROW");
        }
        
        return true;
    }
}
