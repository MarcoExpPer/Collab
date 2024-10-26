

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class ItemSlotHUDWidget : MonoBehaviour
{
    public Color emptySlotColor;
    public Color equipedSlotColor;
    
    private ActiveItemData _activeItemData;
    
    //Cached components
    [SerializeField] [HideInInspector] private RawImage _itemImage;
    [SerializeField] [HideInInspector] private RawImage _backgroundImage;
    [SerializeField] [HideInInspector] private AmmoTracker _ammoTracker;
    
    public void Awake()
    {
        DisableItemSlot();
    }
    
    /*
     * This function is kind of the same as the one in InventoryItemSlotWidget but im not sure how to make that one a child
     * because this one uses a RawImage as background to change its color manualy while the other one uses a Image because
     * its a button and the navigation system what will change its color.
     */
    public void ActivateItemSlotWidget(ActiveItem activeItem)
    {
        _activeItemData = activeItem.ItemData;
        _itemImage.texture = _activeItemData.UIData.texture;

        _ammoTracker.TrackAmmo(activeItem);
        
        _itemImage.enabled = true;
        _backgroundImage.color = equipedSlotColor;
    }

    public void DisableItemSlot()
    {
        _itemImage.enabled = false;
        _ammoTracker.enabled = false;
        _backgroundImage.color = emptySlotColor;
    }
    
    private void OnValidate()
    {   
        RawImage[] images = GetComponentsInChildren<RawImage>();
        //Get component in Children also returns the current gameobject component. WTF Unity
        _itemImage = images[1];
        _backgroundImage = images[0];
        
        _ammoTracker = GetComponentInChildren<AmmoTracker>();
    }
 
}
