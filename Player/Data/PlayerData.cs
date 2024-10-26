using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Player/PlayerData")]
public class PlayerData : ScriptableObject
{ 
    /**
     * PLAYER STATISTICS
     */
    //Seeds
    [SerializeField] public int maxSeeds = 100;
    [HideInInspector] public int seeds = 0;
    //First number is current, second is max
    public UnityEvent<int, int> OnSeedsUpdated = new UnityEvent<int, int>();
    //Llaves
    [HideInInspector] public int goldenKeys = 0;
    public UnityEvent<int> OnGoldenKeysUpdated = new UnityEvent<int>();
    //Semillas
    [HideInInspector] public int crystals = 0;
    public UnityEvent<int> OnCrystalsUpdated = new UnityEvent<int>();
    
    public EntityStatsData EntityStats;
    
    /**
     * Pick and throw is baseline, so player already start with its statistics
     */
    public PickAndThrowData PickAndThrowData;
    
    /**
     * EQUIPMENT
     */
    public ObtainableItem[] ItemsObtained { get; private set;  } = new ObtainableItem[(int) EItemId.BombsAmmo];
    public ActiveItemData[] EquipedItemsData { get; private set; } = new ActiveItemData[(int)EEquipedItemSlot.MAX];

    /* Temporal fix to Dictionaries in scriptable objects not being cleared between execution.
     * Once we do the saving system this may change
     */
    public void ClearData()
    {
        EntityStats.health = 0;
        goldenKeys = 0;
        seeds = 0;
        crystals = 0;
        EquipedItemsData = new ActiveItemData[(int)EEquipedItemSlot.MAX];
        ItemsObtained = new ObtainableItem[(int) EItemId.MAX]; 
    }

    public void ChangeSeeds(int crystalsToAdd)
    {
        seeds = Math.Clamp(seeds + crystalsToAdd, 0, maxSeeds);
        OnSeedsUpdated.Invoke(seeds, maxSeeds);
    }
    public void ChangeKeys(int keysToAdd)
    {
        goldenKeys += keysToAdd;
        OnGoldenKeysUpdated.Invoke(goldenKeys);
    }
    public void RemoveKey()
    {
        if (goldenKeys > 0)
        {
            goldenKeys--;
            OnGoldenKeysUpdated.Invoke(goldenKeys);
        }
    }
    public void ChangeCrystasls(int crystalsToAdd)
    {
        crystals += crystalsToAdd;
        OnCrystalsUpdated.Invoke(crystals);
    }

    public bool HasPassiveItem(EItemId itemId)
    {
        return ItemsObtained[(int)itemId] != null;
    }
}