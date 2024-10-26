using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class ChestGoldenKeyInteraction : InteractionSource
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
        
        GameManager.Instance.playerData.RemoveKey();

        animator.SetTrigger("Opened");
        itemToObtain.ObtainItem(controller.transform.parent.gameObject);
        controller.EndInteraction();
        
        Destroy(gameObject);
    }

    public override bool CanBeInteractedWith()
    {
        if (base.CanBeInteractedWith())
        {
            if (GameManager.Instance.playerData.goldenKeys > 0)
            {
                return true;
            }
            
        }

        return false;
    }
}
