using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class SaveStateController : MonoBehaviour
{
    private PlayerData _playerData;
    [SerializeField] [HideInInspector] private ItemController _itemController;
    
    public void OnValidate()
    {
        _itemController = GetComponent<ItemController>();
    }
    
    public void Start()
    {
        _playerData = GameManager.Instance.playerData;
        
        //First iterate, then obtain to avoid errors of iterating a List while accesing the dictionary
        List<ObtainableItem> aItemsToAdd = new List<ObtainableItem>();
        foreach (var item in _playerData.ItemsObtained)
        {
            if (item != null)
            {
                aItemsToAdd.Add(item);
            }
        }
        
        foreach (var item in aItemsToAdd)
        {
            item.ObtainItem(_itemController.gameObject);
        }
        
        for (int i = 0; i < _playerData.EquipedItemsData.Length; ++i)
        {
            ActiveItemData equipedItem = _playerData.EquipedItemsData[i];
            if (equipedItem)
            {
                _itemController.EquipItem(equipedItem.GetItemId(), (EEquipedItemSlot) i);
            }
        }
    }

    public void SaveGame()
    {
        
    }

    public void LoadGame()
    {
        
    }
    
}
