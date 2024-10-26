using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;


public class FlashController : MonoBehaviour
{
    [Header("Flash Color")] 
    [SerializeField] private bool enableFlashColor = true;
    [ShowIf("enableFlashColor")]
    [SerializeField] private float colorPeriod = 0.25f;
    [SerializeField] private AnimationCurve colorCurve;
    [SerializeField] private Color flashColor;
    [SerializeField] private float colorEffectDuration = 2f;
    private Coroutine colorCoroutine;
    
    [EndIf] [Header("Flash Alpha")] 
    [SerializeField] private bool enableFlashAlpha = true;
    [ShowIf("enableFlashAlpha")]
    [SerializeField] private float alphaPeriod = 0.4f;
    [SerializeField] private float invisibleDuration = 0.25f;
    [SerializeField] private float alphaEffectDuration = 2f;
    private Coroutine alphaCoroutine;
    
    private readonly List<IEffect> colorEffectsList = new();
    private readonly List<IEffect> alphaEffectsList = new();
    
    [SerializeField] private IEffect[] colorEffects;
    [SerializeField] private IEffect[] alphaEffects;
#if UNITY_EDITOR
    public void OnValidate()
    {
        UnityEditor.EditorApplication.delayCall += _OnValidate;
    }

    private void _OnValidate()
    {
        if(!this) return;
        
        SkinnedMeshRenderer[] skmeshes = GetComponentsInChildren<SkinnedMeshRenderer>();
        alphaEffectsList.Clear();
        colorEffectsList.Clear();
        
        foreach (SkinnedMeshRenderer mesh in skmeshes)
        {
            GameObject go = mesh.gameObject;

            OnValidateAlpha(go, mesh);
            OnValidateColor(go, mesh);
        }
        
        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();
        
        foreach (MeshRenderer mesh in meshes)
        {
            GameObject go = mesh.gameObject;
            
            OnValidateAlpha(go, mesh);
            OnValidateColor(go, mesh);
        }
        
        colorEffects = colorEffectsList.ToArray();
        alphaEffects = alphaEffectsList.ToArray();
    }

    private void OnValidateAlpha(GameObject go, Renderer mesh)
    {
        FlashAlpha falpha = go.GetComponent<FlashAlpha>();
        if (!enableFlashAlpha)
        {
            if(falpha)
                DestroyImmediate(falpha, true);
            
        }else
        {
            if (!falpha)
            {
                falpha = go.AddComponent<FlashAlpha>();
            }
            
            falpha.period = alphaPeriod;
            falpha.duration = invisibleDuration;
            falpha.mesh = mesh;
            
            falpha.enabled = false;
            alphaEffectsList.Add(falpha);
        }
    }

    private void OnValidateColor(GameObject go, Renderer mesh)
    {
        FlashColor fcolor = go.GetComponent<FlashColor>();
        if (!enableFlashColor)
        {
            if(fcolor)
                DestroyImmediate(fcolor, true);
        }
        else
        {
            if (!fcolor)
            {
                fcolor = go.AddComponent<FlashColor>();
            }
            fcolor.mesh = mesh;
            fcolor.originalColor = mesh.sharedMaterial.color;
            fcolor.period = colorPeriod;
            fcolor.curve = colorCurve;
            fcolor.targetColor = flashColor;
            
            fcolor.enabled = false;
            colorEffectsList.Add(fcolor);
        }
    }
#endif
    //Starts flash and end it when its duration ends
    public void StartFlash()
    {
        if (colorCoroutine != null)
        {
            StopCoroutine(colorCoroutine);
        }

        if (alphaCoroutine != null)
        {
            StopCoroutine(alphaCoroutine);
        }
        
        foreach (IEffect flashEffect in alphaEffects)
        {
            flashEffect.StartEffect();
        }
        foreach (IEffect flashEffect in colorEffects)
        {
            flashEffect.StartEffect();
        }
        
        colorCoroutine = StartCoroutine(StopFlash(false));
        alphaCoroutine = StartCoroutine(StopFlash(true));
    }
    
    public void ForceStopFlash()
    {
        foreach (IEffect flashEffect in alphaEffects)
        {
            flashEffect.StopEffect(true);
        }
        foreach (IEffect flashEffect in colorEffects)
        {
            flashEffect.StopEffect(true);
        }
        
        if (colorCoroutine != null)
        {
            StopCoroutine(colorCoroutine);
        }

        if (alphaCoroutine != null)
        {
            StopCoroutine(alphaCoroutine);
        }
    }

    IEnumerator StopFlash(bool isAlphaFlash)
    {
        if (isAlphaFlash)
        {
            yield return new WaitForSeconds(alphaEffectDuration);
            
            foreach (IEffect flashEffect in alphaEffects)
            {
                flashEffect.StopEffect(false);
            }
        }
        else
        {
            yield return new WaitForSeconds(colorEffectDuration);
            
            foreach (IEffect flashEffect in colorEffects)
            {
                flashEffect.StopEffect(false);
            }
        }
    }

    private void OnDestroy()
    {
        if (colorCoroutine != null)
        {
            StopCoroutine(colorCoroutine);
        }

        if (alphaCoroutine != null)
        {
            StopCoroutine(alphaCoroutine);
        }
    }
}
