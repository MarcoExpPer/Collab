using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/* 
 * This class manages everything related to items. From items in the inventory to equiped items and their input
 */
public class ItemController : MonoBehaviour
{
    private PlayerData _playerData;
    
    private InventoryPanel _inventoryPanel;
    private EquipedItemsHUD _equipedItemsHUD;

    private readonly Dictionary<EItemId, PassiveItemData> _passiveItems = new Dictionary<EItemId, PassiveItemData>();
    private readonly Dictionary<EItemId, ActiveItem>  _activatableItems = new Dictionary<EItemId, ActiveItem> ();
    private readonly ActiveItem[] _equipedItems = new ActiveItem[(int)EEquipedItemSlot.MAX];
    
    [HideInInspector] public UnityEvent<ERigItemSlot, ActiveItem> onRigSlotUpdated = new UnityEvent<ERigItemSlot, ActiveItem>();

    public void Awake()
    {
        GameManager.Instance.itemController = this;
    }

    public void Start()
    {   
        _playerData = GameManager.Instance.playerData;
        
        _inventoryPanel = GameManager.Instance.uiManager.inventoryPanel;
        _inventoryPanel.itemController = this;
        
        _equipedItemsHUD = GameManager.Instance.uiManager.equipedItemsHUD;
    }
    
    
    /**
     * ITEM CONTROLLER
     */
    public void ItemInput(bool inputValue, EEquipedItemSlot slot)
    {
        ActiveItem itemInSlot = _equipedItems[(int)slot];

        if (itemInSlot != null)
        {
            itemInSlot.OnInput(inputValue);
        }
    }
    
    public void EquipItem(EItemId itemId, EEquipedItemSlot targetSlot)
    {
        //if we are equipong an item in the same slot, we do nothing (this should never happen tho)
        ActiveItem oldItem = _equipedItems[(int)targetSlot];
        if (oldItem && oldItem.ItemData.GetItemId() == itemId) return;
        
        ActiveItem newItem = _activatableItems[itemId];
        
        if (targetSlot <= EEquipedItemSlot.Item1)
        {   
            //Check if the item we are equiping is already in the other slot (in such case we swap item places)
            EEquipedItemSlot otherSlot = targetSlot == EEquipedItemSlot.Item0 ? EEquipedItemSlot.Item1 : EEquipedItemSlot.Item0;
            ActiveItem otherSlotItem = _equipedItems[(int)otherSlot];
           
            if (otherSlotItem == newItem)
            {
                InternalEquipItem(otherSlot, oldItem);
                InternalEquipItem(targetSlot, newItem);
                
                return;
            }
        }
        
        if (oldItem != null)
        {
            oldItem.OnUnEquip();
        }
        
        InternalEquipItem(targetSlot, newItem);
        newItem.OnEquip(targetSlot);
    }

    //Set Interface and player data information
    private void InternalEquipItem(EEquipedItemSlot targetSlot, ActiveItem item)
    {
        _equipedItems[(int)targetSlot] = item;
        _equipedItemsHUD.UpdateSlot(targetSlot, item);
        _playerData.EquipedItemsData[(int)targetSlot] = item ? item.ItemData : null;
    }
    /**
     * OBTAIN ITEMS
     */
    public void ObtainActiveItem(ActiveItem item)
    {
        if (_activatableItems.TryAdd(item.ItemData.GetItemId(), item))
        {
            //If the item was added, it means it was not previously in our playerData, so is the first time we loot this item
            if (_playerData.ItemsObtained[(int) item.ItemData.GetItemId()] == null)
            {
                _playerData.ItemsObtained[(int)item.ItemData.GetItemId()] = item.ItemData;
                item.ItemData.AmmoCount = item.ItemData.MaxAmmo;
            }
            
            //While the sword is an active item, in the UI is in the key items area
            if (item.ItemData.GetItemId() < EItemId.Sword)
            {
                _inventoryPanel.AddEquipableItem(item);
            }
            else
            {
                _inventoryPanel.AddKeyItem(item);
            }
        }
    }
    
    /**
     * PASSIVE ITEMS
     */
    public void ObtainPassiveItem(PassiveItemData item)
    {
        if (_passiveItems.TryAdd(item.GetItemId(), item))
        {
            _playerData.ItemsObtained[(int)item.GetItemId()] = item;
            _inventoryPanel.AddKeyItem(item);
        }
    }
    
    /**
     * ANIMATION EVENTS
     */
    public void SWORD_EVENT_DoAttackDamage()
    {
        ActiveSword activeSword = (ActiveSword) _equipedItems[(int)EEquipedItemSlot.Sword];
        if (activeSword)
        {
            activeSword.DoAttackDamage();
        }
    }
    public void SWORD_EVENT_EndAttackConstraints()
    {
        ActiveSword activeSword = (ActiveSword) _equipedItems[(int)EEquipedItemSlot.Sword];
        if (activeSword)
        {
            activeSword.EndAttackConstraints();
        }
    }

    public ActiveItem GetActiveItem(EItemId itemToGet)
    {
        return _activatableItems[itemToGet];
    }
}
