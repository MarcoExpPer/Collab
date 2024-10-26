using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class ChestInteraction : InteractionSource
{
    public ObtainableItem itemToObtain;
    private Animator animator;
    public override void Awake()
    {
        base.Awake();
        animator = GetComponentInParent<Animator>();
    }

    public override void ExecuteInteraction(InteractionController controller)
    {
        this.Controller = controller;
        
        animator.SetTrigger("Opened");
        itemToObtain.ObtainItem(controller.transform.parent.gameObject);
        controller.EndInteraction();
        
        Destroy(gameObject);
    }
}
