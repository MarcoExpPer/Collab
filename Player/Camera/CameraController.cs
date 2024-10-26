using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private List<CinemachineVirtualCamera> _vCamsStack = new List<CinemachineVirtualCamera>();

    [SerializeField, HideInInspector] private CinemachineVirtualCamera _initialCam;
    private CinemachineVirtualCamera _currentCam;
    
    public void Awake()
    {
        GameManager.Instance.cameraController = this;
    }

    public void PushCameraToStack(CinemachineVirtualCamera cam)
    {
        _currentCam.enabled = false;
        
        _currentCam = cam;
        _vCamsStack.Add(_currentCam);
        _currentCam.enabled = true;
    }

    public void PopCameraFromStack(CinemachineVirtualCamera cam)
    {
        cam.enabled = false;
        _vCamsStack.Remove(cam);
        
        //The new top should be at least the initial camera, because we never pop that one
        CinemachineVirtualCamera topCam = _vCamsStack[^1];
        _currentCam = topCam;
        _currentCam.enabled = true;
    }

    public void SimpleChangeToCamera(CinemachineVirtualCamera cam)
    {
        _currentCam.enabled = false;
        
        _currentCam = cam;
        _currentCam.enabled = true;
    }

    public void SetInitialCamera(CinemachineVirtualCamera cam)
    {
        _initialCam = cam;
        _currentCam = _initialCam;
        PushCameraToStack(_initialCam);
    }
}
