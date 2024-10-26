
using System;
using Unity.VisualScripting;
using UnityEngine;

public class HearingSense : IEnemySense
{
    private HearingSense_SO _senseSO;

    private const string HearingDetectorGameObjectName = "HearingDetector";
    [SerializeField] private PlayerTrigger _hearingDetectorTrigger;
    private bool _isActive = false;

    private GameObject _playerGameObject;

    public HearingSense(GameObject owner, HearingSense_SO senseSO)
    {
        _senseSO = senseSO;
        
        senseSO.colliderInformation.colliderGameObjectName = HearingDetectorGameObjectName;
        _hearingDetectorTrigger = Utils.FindOrCreatePlayerTriggerToGameObject(owner, senseSO.colliderInformation);
        
        _hearingDetectorTrigger.onPlayerEnterTrigger.RemoveAllListeners();
        _hearingDetectorTrigger.onPlayerEnterTrigger.AddListener(OnPlayerEnterTrigger);
        
        _hearingDetectorTrigger.enabled = _isActive;
    }

    private void OnPlayerEnterTrigger(GameObject playerGameObject)
    {
        _playerGameObject = playerGameObject;
    }

    public void SetActive(bool active)
    {
        _isActive = active;
        _hearingDetectorTrigger.enabled = _isActive;
    }
    
    public EEnemySenseResult GetSenseResult()
    {
        return _hearingDetectorTrigger.IsPlayerInside
            ? _senseSO.fullyDetectPlayerOnSensed ? EEnemySenseResult.Success : EEnemySenseResult.Unsure //If enabled
            : EEnemySenseResult.No; //If not enabled
    }

    public Vector3 GetSenseLocation()
    {
        return _playerGameObject.transform.position;
    }

    public void DrawDebugGizmos(GameObject ownerGameObject)
    {
       Gizmos.DrawWireSphere(ownerGameObject.transform.position, _senseSO.colliderInformation.radius);
    }
}
