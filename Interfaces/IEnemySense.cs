
using UnityEngine;

public enum EEnemySenseResult
{
    Success,    //If the sense has "definetly" senses the target
    Unsure,     //Unsure if it has detected the target but owner should change to some sort of alert state
    No,         //If theres no target to be sensed
}
public interface IEnemySense
{
    public EEnemySenseResult GetSenseResult();
    
    public Vector3 GetSenseLocation();
    
    public void DrawDebugGizmos(GameObject ownerGameObject);
    
    public void SetActive(bool active);

    
}
