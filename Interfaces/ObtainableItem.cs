
using System;
using Unity.VisualScripting;
using UnityEngine;

public enum EItemId
{
    //Active Items
    Bombs = 0,
    Bow,
    Candle,
    //Active but Key Items
    Sword,  //Keep the sword the first of key active items
    Shield,
    //Passive Items
    ArmBands,
    KeyDoor,
    PulseraFuerza,
    //Consumable Items
    BombsAmmo, //This must be the first of key consumable items. Used to know how many non consumable items do we have
    Crystals,
    Seeds,
    GoldenKeys,
    HealingItem,
    MAX,
    None
}
/*This technicaly is not an interface, but it worls like that, It is needed because interfaces cant be serialized and so,
 * cant be used as parameters in editor in a script
 */
public abstract class ObtainableItem : ScriptableObject
{
    
    public abstract void ObtainItem(GameObject OwnerGameObject);
    
    public abstract EItemId GetItemId();
    
    // COMPARISON CHECKS
    public static bool operator== (ObtainableItem obj1, ObtainableItem obj2)
    {
        bool isObj1Null = obj1.IsUnityNull();
        bool isObj2Null = obj2.IsUnityNull();
        
        if (isObj2Null && isObj1Null) return true;
        if(isObj1Null || isObj2Null) return false;
        
        return (obj1.GetItemId() == obj2.GetItemId());
    }
    public static bool operator!= (ObtainableItem obj1, ObtainableItem obj2)
    {
        bool isObj1Null = obj1.IsUnityNull();
        bool isObj2Null = obj2.IsUnityNull();
        
        if (isObj2Null && isObj1Null) return false;
        if(isObj1Null || isObj2Null) return true;
        
        return (obj1.GetItemId() != obj2.GetItemId());
    }
    
    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ObtainableItem)obj);
    }
    
    protected bool Equals(ObtainableItem other)
    {
        return base.Equals(other) && GetItemId() == other.GetItemId();
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), (int)GetItemId());
    }
}
