
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DebugItemManager :MonoBehaviour
{
    [SerializeField] private DebugObtainableItemDatabase itemDatabase;
    [SerializeField] private VerticalLayoutGroup consumableGroup;
    [SerializeField] private VerticalLayoutGroup itemsGroup;
    [SerializeField] DebugAddItembtn obtainItemBtnPrefab;
    
    void Awake()
    {
        foreach (ObtainableItem item in itemDatabase.obtainableItems)
        {
            DebugAddItembtn obtainItemBtn;
            //CONSUMABLE
            if (item.GetItemId() >= EItemId.BombsAmmo)
            {
                obtainItemBtn = Instantiate(obtainItemBtnPrefab, consumableGroup.transform);
                
            }
            else
            {
                obtainItemBtn = Instantiate(obtainItemBtnPrefab, itemsGroup.transform);
            }
            
            obtainItemBtn.itemToObtain = item;
            obtainItemBtn.GetComponentInChildren<TextMeshProUGUI>().text = item.name;
        }
    }
}
