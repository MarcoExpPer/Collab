using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerTrigger))]
public class PlayerDamageWhileInTrigger : MonoBehaviour
{
    [SerializeField] private PlayerTrigger _playerTrigger;
    private INewHitable _playerHitable;
    public HitEffect_SO[] hitEffectsSo;
    public float damageTickRate = 0.5f;
    
    Coroutine _damageCoroutine;
    protected virtual void Awake()
    {
        _playerTrigger.onPlayerEnterTrigger.AddListener(OnPlayerEnter);
        _playerTrigger.onPlayerExitTrigger.AddListener(OnPlayerExit);
    }

    private void OnValidate()
    {
        _playerTrigger = GetComponent<PlayerTrigger>();
    }

    IEnumerator DamageTick()
    {
        yield return new WaitForSeconds(damageTickRate);
        _playerHitable.TryHits(hitEffectsSo, gameObject);
        _damageCoroutine = StartCoroutine(DamageTick());
    }
    // Update is called once per frame
    void OnPlayerEnter(GameObject playerGameObject)
    {
        if (_playerHitable != null)
        {
            _playerHitable = playerGameObject.GetComponent<INewHitable>();
        }
        
        _playerHitable.TryHits(hitEffectsSo, gameObject);
        
        _damageCoroutine = StartCoroutine(DamageTick());
    }

    void OnPlayerExit(GameObject playerGameObject)
    {
        StopCoroutine(_damageCoroutine);
    }
};
