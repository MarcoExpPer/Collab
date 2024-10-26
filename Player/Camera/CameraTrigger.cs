
using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using VInspector;

[RequireComponent(typeof(PlayerTrigger))]
public class CameraTrigger : MonoBehaviour
{
    [SerializeField, HideInInspector] private CinemachineVirtualCamera _localCam;
    [SerializeField, HideInInspector] private PlayerTrigger _playerTrigger;
    private CameraController _cameraController;

    public bool isMainCamera = false;
    
    [HideIf("isMainCamera")]
    public bool isStackCamera = false;
    
    
    public void Awake()
    {
        if (isMainCamera)
        {
            isStackCamera = false;
        }
        
        _playerTrigger.onPlayerEnterTrigger.AddListener(OnPlayerEnter);

        if (isStackCamera)
        {
            _playerTrigger.onPlayerExitTrigger.AddListener(OnPlayerExit);  
        }
        
        _localCam.enabled = isMainCamera;
    }

    public void Start()
    {
        _cameraController = GameManager.Instance.cameraController;

        if (isMainCamera)
        {
            GameManager.Instance.cameraController.SetInitialCamera(_localCam);
        }
    }

    private void OnValidate()
    {
        _localCam = GetComponentInChildren<CinemachineVirtualCamera>();
        _playerTrigger = GetComponent<PlayerTrigger>();
    }
    
    void OnPlayerEnter(GameObject playerGameObject)
    {
        if (isStackCamera)
        {
            _cameraController.PushCameraToStack(_localCam);
        }
        else
        {
            _cameraController.SimpleChangeToCamera(_localCam);
        }
    } 
    
    void OnPlayerExit(GameObject playerGameObject)
    {
        _cameraController.PopCameraFromStack(_localCam);
    }
}
