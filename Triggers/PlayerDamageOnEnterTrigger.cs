
using UnityEngine;

[RequireComponent(typeof(PlayerTrigger))]
public class PlayerDamageOnEnterTrigger : MonoBehaviour
{
    [SerializeField] private PlayerTrigger _playerTrigger;
    [SerializeField] private HitEffect_SO hitEffect;
    
    public void Awake()
    {
        _playerTrigger.onPlayerEnterTrigger.AddListener(OnPlayerEnter);
    }

    private void OnValidate()
    {
        _playerTrigger = GetComponent<PlayerTrigger>();
    }
    
    void OnPlayerEnter(GameObject playerGameObject)
    {
        INewHitable hitComponent = playerGameObject.GetComponent<INewHitable>();
        hitComponent.TryHit(hitEffect, gameObject);
    }
};