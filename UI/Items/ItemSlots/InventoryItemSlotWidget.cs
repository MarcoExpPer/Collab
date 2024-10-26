using System;
using Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VInspector;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class InventoryItemSlotWidget : MonoBehaviour, ISelectableUI, IPointerEnterHandler, IDeselectHandler
{
    /*
     * If more parameters need to be public and editable from editor, it may be needed to update ItemSlotWidgetEditor
     */
    //Data containers
    public bool useActiveItemData; // Determines which field to use in the editor
    [HideIf("useActiveItemData")]
    [SerializeField] public PassiveItemData passiveItemData;
    [EndIf]
    [ShowIf("useActiveItemData")]
    [SerializeField] public ActiveItemData activeItemData;
    [EndIf]
    
    //Cached components
    [SerializeField, HideInInspector] private RawImage _itemImage;
    [SerializeField, HideInInspector] private Button _button;
    [SerializeField, HideInInspector] private AmmoTracker _ammoTracker;
    [HideInInspector] public InventoryPanel inventoryPanel;

    private bool isActive = false;
    public void Awake()
    {
        if (!isActive)
        {
            _itemImage.enabled = false;
            _button.interactable = false; 
        }

    }

    public EItemId GetItemID()
    {
        if (activeItemData)
        {
            return activeItemData.GetItemId();
        }

        if (passiveItemData)
        {
            return passiveItemData.GetItemId();
        }
        return EItemId.None;
    }

    public ItemUIData GetItemUIData()
    {
        return activeItemData ? activeItemData.UIData : passiveItemData.UIData;
    }
    
    public void ActivateItemSlotWidget(ActiveItem activeItem)
    {
        activeItemData = activeItem.ItemData;
        _itemImage.texture = activeItemData.UIData.texture;

        _ammoTracker.TrackAmmo(activeItem);
        
        _itemImage.enabled = true;
        _button.interactable = true;
        isActive = true;
    }
    
    public void PassiveItemSlotWidget(PassiveItemData passiveItem)
    {
        passiveItemData = passiveItem;
        _itemImage.texture = passiveItemData.UIData.texture;
        _itemImage.enabled = true;
        _button.interactable = true;
        isActive = true;
    }

    public bool IsSelectable()
    {
        return isActive;
    }
    
    /*
     * ITEM INPUTS
     *
     * Only active items accept inputs.
     */
     
    /*
     * Base items dont accept confirm input.
     *
     * BUT some Zelda games use the confirm input to open a new menu to select between different
     * types of the same ItemSlotWidget. Like, confirm a Bow and then open a dropdown to select normal/fire/ice arrows.
     *
     * So we may use it in the future
     */
    public bool ConfirmInput(bool fromMouse)
    {
         return false;
    }
    
    public bool ItemInput(EEquipedItemSlot itemSlot, bool fromMouse)
    {
        if (!isActive || !useActiveItemData) return false;
        //Sword cant be equiped to normal slots
        if (activeItemData.GetItemId() >= EItemId.Sword) return false;
        
        inventoryPanel.EquipItem(activeItemData.GetItemId(), itemSlot);
        return true;
    }
    
    /*
     *  EDITOR FUNCTIONS
     */
    //Allow to see the results in editor
    private void OnValidate()
    {   
        //Get component in Children also returns the current gameobject component. WTF Unity
        _itemImage = gameObject.GetComponentInChildren<RawImage>();
        _button = gameObject.GetComponent<Button>();
        _ammoTracker = gameObject.GetComponentInChildren<AmmoTracker>();
        
        if (useActiveItemData && activeItemData)
        {
            EDITOR_ShowActiveData();
            _button.interactable = true;
            passiveItemData = null;
        }
        else if(passiveItemData)
        {
            activeItemData = null;
            _button.interactable = true;
            EDITOR_ShowPassiveData();
        }
        else
        {
            EDITOR_NoData();
            _button.interactable = false;
            passiveItemData = null;
            activeItemData = null;
        }
    }
    
    public void EDITOR_ShowPassiveData()
    {
        _itemImage.texture = passiveItemData.UIData.texture;
        _itemImage.enabled = true;
    }
    
    public void EDITOR_ShowActiveData()
    {
        _itemImage.texture = activeItemData.UIData.texture;
        _itemImage.enabled = true;
    }
    
    public void EDITOR_NoData()
    {
        _itemImage.enabled = false;
    }

    /*
     * We override this so it doesn't show the "hovering" settings, as those can be different from the selected used by
     * non mouse controls
     */
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isActive) return;

        if (!EventSystem.current.alreadySelecting)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
    
    public void OnDeselect(BaseEventData eventData)
    {
        GetComponent<Selectable>().OnPointerExit(null);
    }
    
    
}
