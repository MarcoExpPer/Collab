using System;
using UnityEngine;

public class CandleController : MonoBehaviour
{
    public Light CandleLight;
    public float MaxIntensity;
    public AnimationCurve IntensityCurve;
    public float TimeToMaxIntensity = 1f;
    
    private float currentTime;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (enabled && currentTime < TimeToMaxIntensity)
        {
            currentTime += Time.deltaTime;
            CandleLight.intensity = IntensityCurve.Evaluate(currentTime / TimeToMaxIntensity) * MaxIntensity;
        }
    }

    private void OnEnable()
    {
        currentTime = 0;
        enabled = true;
    }

    void OnDisable()
    {
        CandleLight.intensity = 0;
    }
}
