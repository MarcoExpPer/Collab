using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeTrackerHUD : MonoBehaviour
{
    public SingleLifeIcon healthPrefab;
    
    private List<SingleLifeIcon> HealthIcons = new List<SingleLifeIcon>();
    
    public void Setup(EntityStatsData statsData)
    {
        for (int i = 0; i < statsData.maxHealth/2; ++i)
        {
            SingleLifeIcon icon = Instantiate(healthPrefab, transform).GetComponent<SingleLifeIcon>();
            icon.transform.SetParent(transform);
            icon.SetState(ELifeState.Full);

            HealthIcons.Add(icon);
        }
        
        statsData.onHealthChanged.AddListener(OnHealthUpdated);
        OnHealthUpdated(statsData.health, statsData.health, statsData.maxHealth);
    }

    public void OnHealthUpdated(int previousHealth, int curHealth, int maxHealth)
    {
        for (int i = 1; i <= HealthIcons.Count; ++i)
        {
            SingleLifeIcon currentIcon = HealthIcons[i - 1];

            if (curHealth >= i*2)
            {
                currentIcon.SetState(ELifeState.Full);
            }else if (curHealth == i*2 -1)
            {
                currentIcon.SetState(ELifeState.Half);
            }
            else
            {
                currentIcon.SetState(ELifeState.Empty);
            }
        }
    }
}