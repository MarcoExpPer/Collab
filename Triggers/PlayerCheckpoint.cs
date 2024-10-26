
using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerTrigger))]
public class PlayerCheckpoint : MonoBehaviour
{
    [SerializeField] private PlayerTrigger _playerTrigger;
    private PlayerSpawnController _playerSpawnController;
    private Transform spawnTransform;
    
    public bool AllowMultipleCheckpoints = true;
    public bool savePlayerRotation = true;
    
    public void Awake()
    {
        _playerTrigger.onPlayerEnterTrigger.AddListener(OnPlayerEnter);
    }

    public void Start()
    {
        _playerSpawnController = GameManager.Instance.spawnController;
        
        spawnTransform = transform.childCount > 0 ? transform.GetChild(0) : transform;
    }

    private void OnValidate()
    {
        _playerTrigger = GetComponent<PlayerTrigger>();
    }
    
    void OnPlayerEnter(GameObject playerGameObject)
    {
        Quaternion spawnRotation = savePlayerRotation ? playerGameObject.transform.rotation : spawnTransform.rotation;
        _playerSpawnController.UpdateSpawnLocation(spawnTransform.position, spawnRotation);
        
        if (!AllowMultipleCheckpoints)
        {
            Destroy(gameObject);
        }
    }
}
