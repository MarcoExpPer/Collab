using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashColor : MonoBehaviour, IEffect
{
    [SerializeField] public Renderer mesh;
    [SerializeField] public Color originalColor;
    [SerializeField] public Color targetColor;
    
    public float period = 0.25f;
    public AnimationCurve curve;
    
    private float currentTime;
    private bool goingUp = true;

    private bool isLooping = false;

    // Update is called once per frame
    void Update()
    {
        currentTime += goingUp ? +Time.deltaTime : -Time.deltaTime;
        if (currentTime >= period)
        {
            goingUp = false;
        }else if (currentTime < 0)
        {
            if (!isLooping)
            {
                mesh.material.color = originalColor;
                enabled = false;
            }
            goingUp = true;
        }
        
        float alpha = curve.Evaluate(currentTime / period);
        
        mesh.material.color = Color.Lerp(originalColor, targetColor, alpha);
    }

    public void StartEffect()
    {
        currentTime = 0;
        isLooping = true;
        enabled = true;
    }

    public void StopEffect(bool forceStop)
    {
        if (forceStop)
        {
            enabled = false;
        }
        isLooping = false;
    }
    
    private void OnDisable()
    {
        isLooping = false;
        mesh.material.color = originalColor;
    }
}
