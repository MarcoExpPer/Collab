

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


[CreateAssetMenu(menuName = "Nilsh/EntityStats")]
public class EntityStatsData : ScriptableObject
{ 
    [HideInInspector] public int health = 0;
    public int maxHealth = 10;
    
    //First means previous health, second current health and third max health
    [HideInInspector] public UnityEvent<int, int, int> onHealthChanged = new ();
    [HideInInspector] public UnityEvent onHealthZero = new UnityEvent();
    
    public void ChangeHealth(int amountToIncrease)
    {
        int previousHealth = health;
        health = Mathf.Clamp(health + amountToIncrease, 0, maxHealth);
        onHealthChanged.Invoke(previousHealth, health, maxHealth);

        if (health == 0)
        {
            onHealthZero.Invoke();
        }
    }
}