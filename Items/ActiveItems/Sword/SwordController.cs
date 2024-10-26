using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class SwordController : MonoBehaviour
{
    [HideInInspector] public SwordItemData swordData;

    private List<GameObject> EnemiesInsideArea = new List<GameObject>();
    
    private bool isAttacking = false;
    private GroundedInput _groundedInput;
    private Animator _anim;
    [HideInInspector] public BoxCollider slashCollider;
    private void Start()
    {
        _groundedInput = GameManager.Instance.inputManager.groundedInput;
        _anim = GetComponentInParent<Animator>();
    }
    
    public bool TryStartAttackAnimation()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            _anim.SetTrigger(PlayerSMController.SwordSlashTrigger);
        }
        
        return !isAttacking;
    }

    public void DoAttackDamage()
    {
        _groundedInput.enabled = false; 
        
        // Get the center and size in local space
        Vector3 localCenter = slashCollider.center;
        Vector3 size = slashCollider.size * 0.5f; // Half extents are half the size
        
        Vector3 worldCenter = transform.TransformPoint(localCenter);
        Quaternion worldRotation = transform.rotation;
        
        Collider[] results = Physics.OverlapBox(worldCenter, size, worldRotation);
        
        for (int i = 0; i < results.Length; i++)
        {
            Collider hitCollider = results[i];
            if (hitCollider.gameObject.CompareTag(GameManager.HitableTag))
            {
                INewHitable hit = hitCollider.gameObject.GetComponent<INewHitable>();
                if (hit != null)
                {
                    hit.TryHits(swordData.swordEffects, gameObject);
                }
            }
        }
        
    }

    public void EndAttackConstraints()
    {
        _groundedInput.enabled = true;
        isAttacking = false;
    }
}
