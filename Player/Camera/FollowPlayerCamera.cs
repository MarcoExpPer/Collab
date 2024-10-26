using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class FollowPlayerCamera : MonoBehaviour
{
    [SerializeField] [HideInInspector] protected CinemachineVirtualCamera Vcam;
    [FormerlySerializedAs("PlayerTeleportController")] [SerializeField] [HideInInspector] protected PlayerSpawnController playerSpawnController;

    public bool TeleportWithPlayer = true;
    
    private void OnValidate()
    {   
        Vcam = gameObject.GetComponent<CinemachineVirtualCamera>();

        playerSpawnController = FindObjectOfType<PlayerSpawnController>();
        if (playerSpawnController)
        {
            Vcam.Follow = playerSpawnController.transform;
        }
    }

    public void OnEnable()
    {
        if (TeleportWithPlayer)
        {
            playerSpawnController.onPlayerTeleport.AddListener(OnPlayerTeleport);
        }
       
    }

    public void OnDisable()
    {
        if (TeleportWithPlayer)
        {
            playerSpawnController.onPlayerTeleport.RemoveListener(OnPlayerTeleport);
        }
    }

    private void OnPlayerTeleport()
    {
        Vcam.PreviousStateIsValid = false;
    }
}
