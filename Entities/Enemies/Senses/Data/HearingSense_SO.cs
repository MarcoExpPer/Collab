using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(menuName = "Enemies/Senses/Hearing")]
public class HearingSense_SO : Sense_SO
{
    [SerializeField] public Utils.ColliderSpawnInformation colliderInformation;
    public override IEnemySense CreateSenseComponent(GameObject ownerGameObject)
    {
        HearingSense senseComponent = new HearingSense(ownerGameObject, this);
        
        return senseComponent;
    }

    public void OnValidate()
    {
        colliderInformation.isSphereCollider = true;
    }
}
