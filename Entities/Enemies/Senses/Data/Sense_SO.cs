using System;
using UnityEngine;


public abstract class Sense_SO : ScriptableObject
{
    [Tooltip("If false, change to alert state when player is sensed")]
    public bool fullyDetectPlayerOnSensed = false;
    
    public abstract IEnemySense CreateSenseComponent(GameObject ownerGameObject);
    
}
