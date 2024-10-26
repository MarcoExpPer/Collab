using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public enum EEquipedItemSlot
{
    Item0 = 0,
    Item1,
    Sword,
    Shield,
    MAX,
    None
}

public enum ERigItemSlot
{
    SwordHand = 0,
    ShieldHand,
    BothHands,
    MAX,
    None
}

public class ActiveItem : MonoBehaviour
{
    public ItemUIData UIData { get; protected set; }
    public EEquipedItemSlot currentEquipedItemSlot = EEquipedItemSlot.None;
    protected ERigItemSlot CurrentRigSlot = ERigItemSlot.None;
    
    public ActiveItemData ItemData { get; private set; }
    protected ItemController Item;
    protected GameObject RigGameObject;
    
    //First value is the current amount, second is the max amount
    public UnityEvent<int, int> ammoUpdatedEvent = new UnityEvent<int, int>();
    
    public virtual void OnObtained(ActiveItemData itemData)
    {
        UIData = itemData.UIData;
        ItemData = itemData;   

        Item = GetComponent<ItemController>();
        Item.ObtainActiveItem(this);

        if ((int) CurrentRigSlot < (int) EEquipedItemSlot.MAX)
        {
            Item.onRigSlotUpdated.AddListener(HidePrefabWhenSlotOccupied);
        }
    }
    
    //Returns false if it failed any check to accept the input
    public virtual bool OnInput(bool inputValue)
    {
        return ItemData.MaxAmmo <= 0 || (ItemData.MaxAmmo > 0 && ItemData.AmmoCount > 0);
    }
    
    public virtual void OnEquip(EEquipedItemSlot currentSlot)
    {
        this.currentEquipedItemSlot = currentSlot;
    }

    public virtual void OnUnEquip()
    {
        currentEquipedItemSlot = EEquipedItemSlot.None;

        if (RigGameObject)
        {
            RigGameObject.SetActive(false);
        }
    }

    public void ChangeAmmo(int ammo)
    {
        ItemData.AmmoCount = Math.Clamp(ItemData.AmmoCount + ammo, 0, ItemData.MaxAmmo);
        ammoUpdatedEvent.Invoke(ItemData.AmmoCount, ItemData.MaxAmmo);
        Debug.Log(ItemData.AmmoCount);
    }

    public GameObject FindPlayerSlotAndInstantiatePrefab(string tagToFind, GameObject prefab)
    {
        GameObject output = null;

        Transform[] playerSlots = GetComponentsInChildren<Transform>().Where(r => r.CompareTag(tagToFind)).ToArray();
        if (playerSlots.Length == 0)
        {
            Debug.LogError("Player doesnt have a component whose tag is (" + tagToFind+") and coudnt add a prefab to it." + tagToFind + " the tag can be edited in the GameManager");
        }
        else
        {
            Transform playerSlot = playerSlots.First();
            output = Instantiate(prefab, playerSlot).gameObject;
            output.transform.parent = playerSlot;
        }

        RigGameObject = output;
        return output;
    }
    
    public void HidePrefabWhenSlotOccupied(ERigItemSlot slot, ActiveItem activeItem)
    {
        if (activeItem == this) return;
        
        //If we are adding something to my current slot, or if the thing added occupies both hands, or if I occupy both hands
        if (slot == CurrentRigSlot || slot == ERigItemSlot.BothHands || CurrentRigSlot == ERigItemSlot.BothHands)
        {
            RigGameObject.SetActive(false);
        }
    }

    public bool IsRigObjectActive()
    {
        return RigGameObject.activeSelf;
    }
    
    // COMPARISON CHECKS
    public static bool operator== (ActiveItem obj1, ActiveItem obj2)
    {
        bool isObj1Null = obj1.IsUnityNull();
        bool isObj2Null = obj2.IsUnityNull();
        
        if (isObj2Null && isObj1Null) return true;
        if(isObj1Null || isObj2Null) return false;
        
        return (obj1.ItemData.GetItemId() == obj2.ItemData.GetItemId());
    }
    public static bool operator!= (ActiveItem obj1, ActiveItem obj2)
    {
        bool isObj1Null = obj1.IsUnityNull();
        bool isObj2Null = obj2.IsUnityNull();
        
        if (isObj2Null && isObj1Null) return false;
        if(isObj1Null || isObj2Null) return true;
        
        return (obj1.ItemData.GetItemId() != obj2.ItemData.GetItemId());
    }
    
    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ActiveItem)obj);
    }

    private bool Equals(ActiveItem other)
    {
        return base.Equals(other) && Equals(UIData, other.UIData) && Equals(ItemData, other.ItemData);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode());
    }
}