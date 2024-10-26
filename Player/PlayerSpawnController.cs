
using System;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

//Controls player teleports, either at load of an scene, respawn or any other necessary moemnt
public class PlayerSpawnController : MonoBehaviour
{
    public UnityEvent onPlayerTeleport = new UnityEvent();
    
    private Vector3 spawnLocation;
    private Quaternion spawnRotation;

    private CharacterController CharacterController;
    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();

        GameManager.Instance.spawnController = this;
        
        spawnLocation = transform.position;
        spawnRotation = transform.rotation;
    }

    private void OnEnable ()
    {
        GameManager.NotifyTransformToPlayer += SpawnPlayer;
    }
    private void OnDisable ()
    {
        GameManager.NotifyTransformToPlayer -= SpawnPlayer;
    }
    
    private IEnumerator TeleportPlayer(Vector3 posicionSpawn, Quaternion rotacionSpawn)
    {
        // Espera un poco para asegurarte de que la escena esté completamente cargada
        yield return new WaitForEndOfFrame();
        
        CharacterController.enabled = false;
        Transform parentTransform = transform.parent;
        if (parentTransform)
        {
            transform.position = posicionSpawn;
            transform.rotation = rotacionSpawn;
        }
        CharacterController.enabled = true;
        
        spawnLocation = posicionSpawn;
        spawnRotation = rotacionSpawn;
        
        onPlayerTeleport.Invoke();
    }

    private void SpawnPlayer(Vector3 positionToChange, Vector3 rotationToChange)
    {
        StartCoroutine(TeleportPlayer(positionToChange, Quaternion.Euler(rotationToChange)));
    }

    public void RespawnPlayer()
    {
        Time.timeScale = 1;
        GameManager.Instance.playerData.EntityStats.ChangeHealth(GameManager.Instance.playerData.EntityStats.maxHealth);
        StartCoroutine(TeleportPlayer(spawnLocation, spawnRotation));
    }

    public void UpdateSpawnLocation(Vector3 posicionSpawn, Quaternion rotacionSpawn)
    {
        spawnLocation = posicionSpawn;
        spawnRotation = rotacionSpawn;
    }
}
