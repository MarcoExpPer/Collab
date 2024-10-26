using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(SphereCollider))]
public class InteractionController : MonoBehaviour
{
    public Sprite interactionSprite;
    public Transform grabItemsTransform;

    [HideInInspector] public InteractionSource lockedSource;

    private readonly List<InteractionSource> _overlappedSources = new List<InteractionSource>();
    private InteractionSource _closestSource;

    private InputManager _inputManager;
    private AlwaysActiveInput _alwaysActiveInput;

    private bool _enableRecheck = false;
    
    private void Awake()
    {
        _enableRecheck = false;
    }
    
    private void Update()
    {
        if (_enableRecheck)
        {
            UpdateClosestSource();  
        }

    }

    public void EndInteraction()
    {
        _overlappedSources.Remove(lockedSource);
        lockedSource = null;
        _closestSource = null;
        
        if (_overlappedSources.Count > 0)
        {
            _enableRecheck = true;
        }
    }

    public void Interact()
    {
        if (lockedSource)
        {
            lockedSource.ExecuteInteraction(this);
        }
        else if (_closestSource)
        {
            if (_closestSource.LockInteractionObject)
            {
                lockedSource = _closestSource;
                _enableRecheck = false;
            }
            
            _closestSource.ExecuteInteraction(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (lockedSource)
        {
            return;
        }

        InteractionSource otherSource = other.GetComponent<InteractionSource>();
        if (otherSource.CanBeInteractedWith() && otherSource.autoInteract)
        {
            otherSource.ExecuteInteraction(this);

            if (otherSource.LockInteractionObject)
            {
                lockedSource = otherSource;
                _enableRecheck = false;
            }
        }
        else
        {
            _overlappedSources.Add(other.gameObject.GetComponent<InteractionSource>());
            _enableRecheck = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (lockedSource && lockedSource.gameObject == other.gameObject)
        {
            return;
        }

        InteractionSource otherSource = other.GetComponent<InteractionSource>();
        if (otherSource.autoInteract)
        {
            otherSource.ExecuteInteractionEnd();
        }

        _overlappedSources.Remove(other.gameObject.GetComponent<InteractionSource>());

        if (_overlappedSources.Count == 0)
        {
            _enableRecheck = false;
        }

        UpdateClosestSource();
    }

    private void UpdateClosestSource()
    {
        InteractionSource newClosestSource = CalculateClosestSource();

        if (newClosestSource != _closestSource)
        {
            if (_closestSource)
            {
                _closestSource.HideInteractionWidget();
            }

            _closestSource = newClosestSource;

            if (_closestSource)
            {
                _closestSource.ShowInteractionWidget(interactionSprite);
            }
        }
    }

    private InteractionSource CalculateClosestSource()
    {
        InteractionSource NewClosestSource = null;
        float ClosestDistance = Mathf.Infinity;

        List<InteractionSource> SourcesToRemove = new List<InteractionSource>();

        foreach (InteractionSource Source in _overlappedSources)
        {
            if (Source)
            {
                if (Source.CanBeInteractedWith())
                {
                    float currentDistance = (transform.position - Source.transform.position).sqrMagnitude;
                    if (currentDistance < ClosestDistance)
                    {
                        NewClosestSource = Source;
                        ClosestDistance = currentDistance;
                    }
                }
            }
            else
            {
                SourcesToRemove.Add(Source);
            }
        }

        foreach (InteractionSource Source in SourcesToRemove)
        {
            _overlappedSources.Remove(Source);
        }

        return NewClosestSource;
    }
}