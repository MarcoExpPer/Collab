
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class NavAgentKnockbackController: MonoBehaviour, IKnockbackable
{
    [SerializeField,HideInInspector] private NavMeshAgent agent;
    [SerializeField,HideInInspector] private Rigidbody rb;
    //The higher this value, the faster will the knockback stop
    [SerializeField] private float knockbackSlownessSpeed = 5;
    
    [HideInInspector] public UnityEvent onKnockbackFinished = new();
    
    private Vector3 dir = Vector3.zero;
    private float force = 0;
    public void OnValidate()
    {
        agent = GetComponentInChildren<NavMeshAgent>();
        rb = GetComponentInChildren<Rigidbody>();
    }

    public void ApplyImpulse(Vector3 impulseDir, float inForce)
    {
        agent.enabled = false;
        force = inForce / rb.mass;
        dir = impulseDir;
        enabled = true;
    }

    public void ForceEndKnockback()
    {
        dir = Vector3.zero;
        enabled = false;
        agent.enabled = true;
        onKnockbackFinished.Invoke();
    }
    
    public void Update(){

        if (force > 0.1f)
        {
            rb.transform.position += dir * (force * Time.deltaTime);
            force = Mathf.Lerp(force, 0f, 5*Time.deltaTime);
        }
        else
        {
            ForceEndKnockback();
        }
    }
}
