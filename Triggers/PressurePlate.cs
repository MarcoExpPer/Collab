

using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PressurePlate : MonoBehaviour
{
    [SerializeField] private bool lockPressurePlate = false;
    [HideInInspector] public bool IsPressed { get; private set; }
    
    [SerializeField] public Collider col;
    [SerializeField] public GameObject itemToSpawn;
    
    private List<GameObject> triggeringItems = new List<GameObject>();
    private void OnValidate()
    {
        col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void Awake()
    {
        itemToSpawn.SetActive(false);
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameManager.PressurePlateTag))
        {
            triggeringItems.Add(other.gameObject);
            OnPressurePlateTriggered();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(GameManager.PressurePlateTag))
        {
            triggeringItems.Remove(other.gameObject);
            OnPressurePlateExit();
        }
    }
    
    private void OnPressurePlateTriggered()
    {
        if (lockPressurePlate)
        {
            Destroy(col);
        }

        if (!IsPressed)
        {
            ExecuteTrigger();
        }
    }
    
    private void OnPressurePlateExit()
    {
        if (triggeringItems.Count == 0)
        {
            ExitTrigger();
        }
    }
    

    private void ExecuteTrigger()
    {            
        IsPressed = true;
        itemToSpawn.SetActive(true);
    }

    private void ExitTrigger()
    {
        IsPressed = false;
        itemToSpawn.SetActive(false);
    }
}