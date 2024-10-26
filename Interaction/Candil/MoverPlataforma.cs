using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverPlataforma : MonoBehaviour
{
    private int totalCandiles = 0;
    private int candilesEncendidos = 0;
    public List<GameObject> candiles = new List<GameObject>();

    private void Start()
    {
        candilesEncendidos = 0;

        foreach (var candil in candiles)
        {
            var candilComponent = candil.GetComponentInChildren<EncenderCandilFijo>();
            if (candilComponent != null)
            {
                totalCandiles++;
                SetPlatformController(candilComponent);
            }
        }
    }

    public void CandilEncendido()
    {
        candilesEncendidos++;
        if (candilesEncendidos >= totalCandiles)
        {
            BajarPlataforma();
        }
    }

    private void BajarPlataforma()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("isMoving", true); 
        }
    }

    public void SetPlatformController(EncenderCandilFijo candil)
    {
        candil.SetplataformaController(this);
    }
}
