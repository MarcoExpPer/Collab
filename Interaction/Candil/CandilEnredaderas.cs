using System;
using System.Linq;
using UnityEngine;
using VInspector;
using UnityEngine.Serialization;

public class CandilEnredaderas  : InteractionSource
{
    
    public override void ExecuteInteraction(InteractionController controller)
    {
        controller.EndInteraction();
        Destroy(transform.parent.gameObject);
    }

    public override bool CanBeInteractedWith()
    {
        if (base.CanBeInteractedWith())
        {
            bool isEquipped = Utils.DoesArrayContainT<CandleItemData, ActiveItemData>(GameManager.Instance.playerData.EquipedItemsData);;

            if (isEquipped)
            {
                return GameManager.Instance.itemController.GetActiveItem(EItemId.Candle).IsRigObjectActive();
            }
            
        }

        return false;
    }
}
