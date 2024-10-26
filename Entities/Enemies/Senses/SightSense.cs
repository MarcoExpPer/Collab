
using System;
using Unity.VisualScripting;
using UnityEngine;

public class SightSense : IEnemySense
{
    [SerializeField] private GameObject _owner;
    [SerializeField] private SightSense_SO _senseSO;
    
    private EEnemySenseResult _senseResult;
    
    private const string SightDetectorGameObjectName = "SightDetector";
    [SerializeField] private PlayerTrigger _sightDetectorTrigger;
    
    private bool _isActive = false;
    
    private Vector3 _playerPosition;

    public SightSense(GameObject owner, SightSense_SO senseSO)
    {
        _owner = owner;
        _senseSO = senseSO;
        
        senseSO.colliderInformation.colliderGameObjectName = SightDetectorGameObjectName;
        _sightDetectorTrigger = Utils.FindOrCreatePlayerTriggerToGameObject(owner, senseSO.colliderInformation);
        _sightDetectorTrigger.enabled = _isActive;
    }

    public void SetActive(bool active)
    {
        _isActive = active;
        _sightDetectorTrigger.enabled = _isActive;
    }
    
    private EEnemySenseResult FindPlayerInView()
    {
        _senseResult = EEnemySenseResult.No;
        if(!_sightDetectorTrigger.IsPlayerInside) return _senseResult;
        
        _playerPosition = GameManager.Instance.itemController.transform.position;
        Vector3 dirToPlayer = (_playerPosition - _owner.transform.position).normalized;
        
       if (Vector3.Angle(_owner.transform.forward, dirToPlayer) < _senseSO.viewDegreesAngle / 2) {
            float distToPlayer = Vector3.Distance (_owner.transform.position, _playerPosition);

            if (!Physics.Raycast (_owner.transform.position, dirToPlayer, distToPlayer, _senseSO.raycastObstacleMask.value ))
            {
                _senseResult = _senseSO.fullyDetectPlayerOnSensed ? EEnemySenseResult.Success : EEnemySenseResult.Unsure;
                return _senseResult;
            }
       }
        
        return _senseResult;
    }

    public Vector3 GetSenseLocation()
    {
        return _playerPosition;
    }

    public void DrawDebugGizmos(GameObject ownerGameObject)
    {
        Transform ownerTransform = ownerGameObject.transform;
        
        float angle = _senseSO.viewDegreesAngle;
        float rayRange = _senseSO.colliderInformation.radius;
        float halfFOV = angle / 2.0f;

        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV - 90, ownerTransform.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV - 90, ownerTransform.up);

        Vector3 rightRayDirection = leftRayRotation * ownerTransform.right * rayRange;
        Vector3 leftRayDirection = rightRayRotation * ownerTransform.right * rayRange;
        
        Gizmos.color = _senseResult == EEnemySenseResult.No ? Color.red : Color.green;
        Gizmos.DrawRay(ownerTransform.position, rightRayDirection);
        Gizmos.DrawRay(ownerTransform.position, leftRayDirection);
        Gizmos.DrawLine(ownerTransform.position + leftRayDirection, ownerTransform.position + rightRayDirection);
    }

    public EEnemySenseResult GetSenseResult()
    {
        return FindPlayerInView();
    }
}
