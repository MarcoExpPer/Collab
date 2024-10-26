using UnityEngine;


public abstract class PassiveItemData : ObtainableItem
{
    public ItemUIData UIData;
    
    public override void ObtainItem(GameObject ownerGameObject)
    {
        ownerGameObject.GetComponent<ItemController>().ObtainPassiveItem(this);
    }
}


