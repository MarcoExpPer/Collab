
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ItemSlotsContainer : MonoBehaviour
{
   [SerializeField] private InventoryItemSlotWidget[] _itemSlots = new InventoryItemSlotWidget[(int) EItemId.MAX];
   
   public void SetupItem(ActiveItem activeItem)
   {
      _itemSlots[(int) activeItem.ItemData.GetItemId()].ActivateItemSlotWidget(activeItem);
   }
   
   public void SetupItem(PassiveItemData passiveItem)
   {
      //_itemSlots[(int) passiveItem.GetItemId()].PassiveItemSlotWidget(passiveItem);
      _itemSlots[(int) passiveItem.GetItemId()].PassiveItemSlotWidget(passiveItem);
   }

   public void OnValidate()
   {
      InventoryPanel inventoryPanel = GetComponentInParent<InventoryPanel>();
      
      foreach (InventoryItemSlotWidget itemSlot in GetComponentsInChildren<InventoryItemSlotWidget>())
      {
         if(itemSlot.GetItemID() != EItemId.None)
         {
            _itemSlots[(int) itemSlot.GetItemID()] = itemSlot;
            _itemSlots[(int) itemSlot.GetItemID()].inventoryPanel = inventoryPanel;
         }
      }
   }
}