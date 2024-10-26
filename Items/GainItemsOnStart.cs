using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class GainItemsOnStart : MonoBehaviour
{
    [SerializeField] public List<PassiveItemData> passiveItems;
    [SerializeField] public List<ActiveItemData> activeItems;

    public void Start()
    {
        foreach (PassiveItemData passiveItem in passiveItems)
        {
             passiveItem.ObtainItem(gameObject);
        }
        
        foreach (ActiveItemData activeItem in activeItems)
        {
            activeItem.ObtainItem(gameObject);
        }

    }
}
