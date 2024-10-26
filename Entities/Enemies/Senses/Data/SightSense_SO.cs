using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(menuName = "Enemies/Senses/Sight")]
public class SightSense_SO : Sense_SO
{
    [Range(0,360)]
    public float viewDegreesAngle;
    
    [SerializeField] public LayerMask raycastObstacleMask;
    [SerializeField] public Utils.ColliderSpawnInformation colliderInformation;
    public override IEnemySense CreateSenseComponent(GameObject ownerGameObject)
    {
        return new SightSense(ownerGameObject, this);
    }

    public void OnValidate()
    {
        colliderInformation.isSphereCollider = true;
    }
}
