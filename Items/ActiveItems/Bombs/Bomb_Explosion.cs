using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Bomb_Explosion : MonoBehaviour
{
    public GameObject bomb;
    public float power = 10.0f;
    public float radius = 5.0f;
    public float upForce = 1.0f;
    public GameObject bigexplosionPrefab;

    public HitEffect_SO[] hitEffectsSo;
    
    //[FormerlySerializedAs("DamageType")] public EOldDamageType oldDamageType = EOldDamageType.normal;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (bomb == enabled)
        {
            Invoke("Detonate", 5); 
        }
    }

    void Detonate()
    {
        Instantiate(bigexplosionPrefab, transform.position, transform.rotation);
        Vector3 explosionPosition = bomb.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);
        foreach(Collider hit in colliders) 
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.AddExplosionForce(power, explosionPosition, radius, upForce, ForceMode.Impulse);
            }
        }
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                collider.GetComponent<INewHitable>().TryHits(hitEffectsSo, gameObject);
            }
            Destroy(gameObject);
        }
    }
}
