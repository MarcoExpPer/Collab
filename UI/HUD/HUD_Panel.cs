
using System;
using UnityEngine;

public class HUD_Panel : MonoBehaviour
{
    [SerializeField] [HideInInspector] private LifeTrackerHUD lifeTrackerHUD;
    [SerializeField] [HideInInspector] private EquipedItemsHUD equipedItemsHUD;
    [SerializeField] [HideInInspector] private ConsumablesPanel consumablesPanel;
    
    public void Awake()
    {
        GameManager.Instance.uiManager.hudPanel = this;
    }

    public void Start()
    {
        lifeTrackerHUD.Setup(GameManager.Instance.playerData.EntityStats);
        consumablesPanel.SetupPlayerData(GameManager.Instance.playerData);
    }

    private void OnValidate()
    {
        lifeTrackerHUD = GetComponentInChildren<LifeTrackerHUD>();
        equipedItemsHUD = GetComponentInChildren<EquipedItemsHUD>();
        consumablesPanel = GetComponentInChildren<ConsumablesPanel>();
    }
}
