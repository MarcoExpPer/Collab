using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AlertSprite : MonoBehaviour
{
    private SpriteRenderer sr;

    public void Awake()
    {
        sr  = gameObject.GetComponent<SpriteRenderer>();
        enabled = false;
    }

    public void Setup()
    {

    }

    private void OnEnable()
    {
        sr.enabled = true;
    }

    private void OnDisable()
    {
        sr.enabled = false;
    }
    
    public void Update()
    {
        transform.LookAt(Camera.main.transform.position, Vector3.up);
    }
}
