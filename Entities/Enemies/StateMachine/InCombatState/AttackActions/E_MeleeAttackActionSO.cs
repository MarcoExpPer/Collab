
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Enemies/InCombat/AttackActions/MeleeAttack")]
public class E_MeleeAttackActionSO : E_AttackActionSO
{
    [Tooltip("If an enemy has different melee attacks, they must have different names")]
    [SerializeField] public string specificActionName = "A";
    private string _meleeTriggerGameObjectName = "meleeTriggerGameObjectName";
    
    [SerializeField] private GameObject _attackColliderGO;
    private Collider _attackCollider;
    
    [SerializeField] private Utils.ColliderSpawnInformation colliderSpawnInformation;
    [SerializeField] public LayerMask AttackLayerMask;
    [SerializeField] private HitEffect_SO[] attackEffects;
    
    private Coroutine _noAnimationAttack;
    private Coroutine _colliderToggle;
    public override void OnValidateSetup(EnemyBrain InEnemyBrain)
    {
        base.OnValidateSetup(InEnemyBrain);
        
        _meleeTriggerGameObjectName += specificActionName;
        colliderSpawnInformation.colliderGameObjectName = _meleeTriggerGameObjectName;
        
        GameObject parentGameObject = null;
        foreach (Transform child in InEnemyBrain.gameObject.transform)
        {
            if (child.gameObject.name == colliderSpawnInformation.colliderGameObjectName)
            {
                parentGameObject = child.gameObject;
            }
        }

        if (!parentGameObject)
        {
            Type colliderType = colliderSpawnInformation.isSphereCollider ? typeof(SphereCollider) : typeof(BoxCollider);
            Type[] components = { colliderType };

            parentGameObject = new GameObject(colliderSpawnInformation.colliderGameObjectName, components);
            parentGameObject.transform.SetParent(InEnemyBrain.gameObject.transform);
            parentGameObject.transform.localPosition = Vector3.zero;
        }
        
        _attackColliderGO = parentGameObject;
        _attackColliderGO.SetActive(false);
    }

    public override void OnRuntimeSetup(EnemyBrain enemyBrain)
    {
        _attackCollider = _attackColliderGO.GetComponent<Collider>();
    }
    
    public override void StartAttack()
    {
        base.StartAttack();

        if (!hasAnimation)
        {
            _noAnimationAttack = enemyBrain.StartCoroutine(NoAnimationAttack());
        }
    }

    private IEnumerator NoAnimationAttack()
    {
        DoAttackEffects();
        yield return new WaitForNextFrameUnit();
        yield return new WaitForNextFrameUnit();
        FinishAttack();
    }
    
    public override void OnAttackStateExit()
    {
        base.OnAttackStateExit();
        
        if (_noAnimationAttack != null)
        {
            enemyBrain.StopCoroutine(_noAnimationAttack);
        }

        if (_colliderToggle != null)
        {
            enemyBrain.StopCoroutine(_colliderToggle); 
        }
    }

    public override void DoAttackEffects()
    {
        base.DoAttackEffects();

        Collider[] hits = new Collider[1];
        if (colliderSpawnInformation.isSphereCollider)
        {
            SphereCollider col = (SphereCollider) _attackCollider;
            Vector3 worldCenter = col.transform.TransformPoint(col.center);
            Physics.OverlapSphereNonAlloc(worldCenter, col.radius, hits, AttackLayerMask);
        }
        else
        {
            BoxCollider col = (BoxCollider) _attackCollider;
            
            Vector3 worldCenter = col.transform.TransformPoint(col.center);
            Vector3 halfExtents = Vector3.Scale(col.size / 2f, col.transform.lossyScale);
            Quaternion orientation = col.transform.rotation;
            
            Physics.OverlapBoxNonAlloc(worldCenter, halfExtents, hits,
                orientation, AttackLayerMask);
        }
        
        foreach (Collider colHit in hits)
        {
            if (colHit)
            {
               colHit.gameObject.GetComponent<INewHitable>().TryHits(attackEffects, enemyBrain.gameObject);
            }
        }
    }
}
