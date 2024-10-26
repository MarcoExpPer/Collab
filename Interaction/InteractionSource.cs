using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(SphereCollider))]
public class InteractionSource : MonoBehaviour
{
    public bool autoInteract = false;
    public bool LockInteractionObject = false;
    
    protected InteractionController Controller;
    protected SpriteRenderer SpriteRenderer;
    
    public void OnValidate()
    {
        GetComponent<SphereCollider>().isTrigger = true;
    }

    public virtual void Awake()
    {
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (SpriteRenderer)
        {
            SpriteRenderer.enabled = false;
        }
    }

    public virtual void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactables");
    }

    public virtual void ExecuteInteraction(InteractionController controller)
    {
        this.Controller = controller;
    }
    
    //Only for autointeracted sources
    public virtual void ExecuteInteractionEnd()
    {
        
    }

    public virtual void ShowInteractionWidget(Sprite sprite)
    {
        if (SpriteRenderer)
        {
            SpriteRenderer.sprite = sprite;
            SpriteRenderer.enabled = true;
        }
    }

    public void HideInteractionWidget()
    {
        if (SpriteRenderer)
        {
            SpriteRenderer.enabled = false;
        }
    }

    public virtual bool CanBeInteractedWith()
    {
        return isActiveAndEnabled;
    }
    
}
