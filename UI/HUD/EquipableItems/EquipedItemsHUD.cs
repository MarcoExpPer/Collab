using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipedItemsHUD : MonoBehaviour
{
    [SerializeField] private List<ItemSlotHUDWidget> EquipedItemsSlot;
    // Start is called before the first frame update

    private void Awake()
    {
        GameManager.Instance.uiManager.equipedItemsHUD = this;
    }

    public void UpdateSlot(EEquipedItemSlot slot, ActiveItem NewItem)
    {
        if (EquipedItemsSlot.Count > (int)slot)
        {
            ItemSlotHUDWidget itemSlot = EquipedItemsSlot[(int)slot];
            if (NewItem == null)
            {
                itemSlot.DisableItemSlot();
            }
            else
            {
                itemSlot.ActivateItemSlotWidget(NewItem);
            }
        }
    }

    private void OnValidate()
    {
        EquipedItemsSlot = new List<ItemSlotHUDWidget>();

        foreach (var itemSlot in GetComponentsInChildren<ItemSlotHUDWidget>())
        {
            EquipedItemsSlot.Add(itemSlot);
        }
    }
}
