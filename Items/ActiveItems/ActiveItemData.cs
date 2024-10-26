using UnityEngine;

public abstract class ActiveItemData : ObtainableItem
{
    public ItemUIData UIData;
    
    [Tooltip("A positive value means it uses ammo, 0 or -1 means it doesnt")]
    public int MaxAmmo = -1;
    [HideInInspector] public int AmmoCount;
    
    public override void ObtainItem(GameObject ownerGameObject)
    {
        ownerGameObject.AddComponent<ActiveItem>().OnObtained(this);
    }
    
}


