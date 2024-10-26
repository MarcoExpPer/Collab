
using System;
using UnityEngine;

public class PlayerKnockbackController: MonoBehaviour, IKnockbackable
{
    [SerializeField,HideInInspector] private CharacterController cc;
    [SerializeField] private float mass = 3;
    //The higher this value, the faster will the knockback stop
    [SerializeField] private float knockbackSlownessSpeed = 5;
    private Vector3 impact = Vector3.zero;
    public void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    public void ApplyImpulse(Vector3 impulseDir, float force)
    {
        impact += impulseDir * force / mass;
        enabled = true;
    }

    public void ForceEndKnockback()
    {
        impact = Vector3.zero;
        enabled = false;
    }
    
    public void Update(){

        if (impact.magnitude > 0.1)
        {
            cc.Move(impact * Time.deltaTime);
            impact = Vector3.Lerp(impact, Vector3.zero, Time.deltaTime);
        }
        else
        {
            ForceEndKnockback();
        }
    }
}
