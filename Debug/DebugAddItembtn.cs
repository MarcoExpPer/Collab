
using System;
using TMPro;
using UnityEngine;

public class DebugAddItembtn : DebugButton
{
    [SerializeField, HideInInspector] public ObtainableItem itemToObtain;
    
    public override void OnClick()
    {
        int itemId = (int)itemToObtain.GetItemId();
        if (itemId >= (int) EItemId.BombsAmmo || GameManager.Instance.playerData.ItemsObtained[itemId] == null)
        {
            itemToObtain.ObtainItem(GameManager.Instance.itemController.gameObject);
        }
    }
}
