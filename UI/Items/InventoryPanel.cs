
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Interfaces;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour, MainUIPanel
{
   public TextMeshProUGUI itemDescriptionText;
   [Tooltip("This slot will be the one selected the first time the player opens this panel. After that, the selected item will" +
            "be the last that the player used")]
   private InventoryItemSlotWidget currentSelected;
   
   [SerializeField] [HideInInspector] private KeyItemsWidget _keyItemsWidget;
   [SerializeField] [HideInInspector] private EquipableItemsWidget _equipableItemsWidget;
   
   [HideInInspector] public ItemController itemController;
   public UnityEvent<bool> OnInventoryToggle = new UnityEvent<bool>();
   
   public void Awake()
   {
      GameManager.Instance.uiManager.inventoryPanel = this;
      gameObject.SetActive(false);
   }
   
   /**
    * SETUP FUNCTIONS CALLED FROM INVENTORY CONTROLLER
    */
   public void AddEquipableItem(ActiveItem activeItem)
   {
      _equipableItemsWidget.SetupItem(activeItem);
   }
   public void AddKeyItem(ActiveItem activeItem)
   {
      _keyItemsWidget.SetupItem(activeItem);
   }
   public void AddKeyItem(PassiveItemData passiveItemData)
   {
      _keyItemsWidget.SetupItem(passiveItemData);
   }

   public void Toggle(bool setActive)
   {
      gameObject.SetActive(setActive);
      OnInventoryToggle.Invoke(setActive);
   }
   
   /*
    * SELECTED SLOT BEHAVIOUR
    */
   public void OnEnable()
   {
      if (currentSelected == null)
      {
         InventoryItemSlotWidget initialInventoryItemSlot = null;
            
         foreach(InventoryItemSlotWidget itemSlot in  GetComponentsInChildren<InventoryItemSlotWidget>())
         {
            if (itemSlot.IsSelectable())
            {
               initialInventoryItemSlot = itemSlot;
               break;
            }
         }

         if (initialInventoryItemSlot != null)
         {
            EventSystem.current.SetSelectedGameObject(initialInventoryItemSlot.gameObject, null);
            currentSelected = initialInventoryItemSlot;
         }
      }
      else
      {
         EventSystem.current.SetSelectedGameObject(currentSelected.gameObject, null);
      }
      
      UpdateDescriptionText(currentSelected);
   }
   
   public void Update()
   {
      if (!EventSystem.current.currentSelectedGameObject) return;
      
      InventoryItemSlotWidget newSelected = EventSystem.current.currentSelectedGameObject.GetComponent<InventoryItemSlotWidget>();
      //GetComponent in an update is a bad idea, but we are in a static UI with the game paused, so it should be fine
      if (newSelected&& newSelected != currentSelected)
      {
         currentSelected = newSelected;
         UpdateDescriptionText(currentSelected);
      }
   }

   public void EquipItem(EItemId itemId, EEquipedItemSlot slot)
   {
      itemController.EquipItem(itemId, slot);
   }
   
   private void UpdateDescriptionText(InventoryItemSlotWidget inventoryItemSlotWidget)
   {
      if (inventoryItemSlotWidget != null)
      {
         itemDescriptionText.text = inventoryItemSlotWidget.GetItemUIData().itemName + "\n" + inventoryItemSlotWidget.GetItemUIData().itemDescription;
      }
      else
      {
         itemDescriptionText.text = "";
      }
   }
   
   private void OnValidate()
   {
      _keyItemsWidget = GetComponentInChildren<KeyItemsWidget>();
      _equipableItemsWidget = GetComponentInChildren<EquipableItemsWidget>();
   }

   public bool ConfirmInput(bool fromMouse)
   {
      //Confirm does nothing here
      return false;
   }

   public bool ItemInput(EEquipedItemSlot itemSlot, bool fromMouse)
   {
      if (fromMouse)
      {
         ISelectableUI slotHit = UIManager.GetSelectableUiUnderMouse<ISelectableUI>();
         if (slotHit == null || slotHit.GetType() != typeof(InventoryItemSlotWidget) || !slotHit.IsSelectable())
         {
            return false;
         }
         
         InventoryItemSlotWidget slotWidget = EventSystem.current.currentSelectedGameObject.GetComponent<InventoryItemSlotWidget>();
         if (slotWidget)
         {
            slotWidget.ItemInput(itemSlot, true);
            
         }
      }
      else
      {
         if (!EventSystem.current.currentSelectedGameObject) return false;
        
         InventoryItemSlotWidget slotWidget = EventSystem.current.currentSelectedGameObject.GetComponent<InventoryItemSlotWidget>();
         if (slotWidget)
         {
            slotWidget.ItemInput(itemSlot, false);
         }
         else
         {
            return false;
         }
      }

      return true;
   }

   public bool IsSelectable()
   {
      return isActiveAndEnabled;
   }
}