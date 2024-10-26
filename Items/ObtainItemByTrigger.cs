using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(PlayerTrigger))]
public class ObtainItemByTrigger : MonoBehaviour
{
    public ObtainableItem itemToObtain;

    void Awake()
    {
        if (itemToObtain == null)
        {
            Debug.LogError("Obtainable by Trigger (" + gameObject.name + ") has no ItemToObtain.");
            return;
        }
        
        PlayerTrigger playerTrigger = GetComponent<PlayerTrigger>();
        playerTrigger.onPlayerEnterTrigger.AddListener(ExecuteObtain);
    }
    
    // Update is called once per frame
    void ExecuteObtain(GameObject obtainedBy)
    {
        itemToObtain.ObtainItem(obtainedBy);
        Destroy(gameObject);
    }
}
