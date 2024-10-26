using UnityEngine;

public class BombInteraction : MonoBehaviour //TODO
{
    public float interactionCooldown = 2.0f;
    public float throwForce = 5.0f;
    public float throwUpwardForce = 10.0f;
    private Rigidbody rb;
    private bool interactable;
    public Transform playerTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        interactable = true;

        if(playerTransform == null)
        {
            playerTransform = GameObject.FindWithTag("Player").transform;
        }
    }


    /*public bool TryDoHit(int damageAmount, EOldDamageType oldDamageType)
    { 
        if(interactable)
        {
            interactable = false;
            Vector3 directionToBomb = (transform.position - playerTransform.position).normalized;
            ImpulseBomb(directionToBomb);
        }
        return true;
    }*/

    private void ImpulseBomb(Vector3 direction)
    {
        Vector3 forceToAdd = direction * throwForce + Vector3.up * throwUpwardForce;
        
        rb.AddForce(forceToAdd, ForceMode.Impulse);

        Invoke(nameof(ResetInteractable), interactionCooldown);
    }

    private void ResetInteractable()
    {
        interactable = true;
    }
}

