
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] public UnityEvent<GameObject> onPlayerEnterTrigger = new UnityEvent<GameObject>();
    [SerializeField] public UnityEvent<GameObject> onPlayerExitTrigger = new UnityEvent<GameObject>();
    public bool IsPlayerInside { get; private set; } = false;
    
    private GameObject _player;

    [SerializeField] public Collider col;
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        col = GetComponent<Collider>();
        col.isTrigger = true;
        //To avoid send message cannot be called during onvalidate when setting gameobject layer, despite being able to do that without errors
        UnityEditor.EditorApplication.delayCall += _OnValidate;
    }

    private void _OnValidate()
    {
        if (!this) return;
        gameObject.layer = GameManager.PlayerDetectorLayer;
    }
#endif
    private void OnEnable()
    {
        if (IsPlayerInside)
        {
            onPlayerEnterTrigger.Invoke(_player);
        }
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        IsPlayerInside = true;
        _player = other.gameObject;
        
        if (enabled)
        {
            onPlayerEnterTrigger.Invoke(_player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IsPlayerInside = false;
        
        if (enabled)
        {
            onPlayerExitTrigger.Invoke(_player);
        }
    }
};
