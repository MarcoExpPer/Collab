using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class DigInteraction : InteractionSource
{
    private Animator animator;
    public bool hasReward;
    private SpawnReward spawnReward;
    public override void Awake()
    {
        base.Awake();
        animator = transform.parent.GetComponent<Animator>();
    }

    public override void ExecuteInteraction(InteractionController controller)
    {
        this.Controller = controller;
        
        if (GameManager.Instance.playerData.EquipedItemsData[(int)EEquipedItemSlot.Sword])
        {
            animator.SetTrigger("Digged");
            if (hasReward)
            {
                spawnReward = GetComponent<SpawnReward>();
                spawnReward.Spawn();
            }
            controller.EndInteraction();
            
            //Do digging effects
            Destroy(gameObject);
        }
    }
    
    public override void ShowInteractionWidget(Sprite sprite)
    {
        if (SpriteRenderer && GameManager.Instance.playerData.EquipedItemsData[(int)EEquipedItemSlot.Sword])
        {
            SpriteRenderer.sprite = sprite;
            SpriteRenderer.enabled = true;
        }
    }

    public override bool CanBeInteractedWith()
    {
        return base.CanBeInteractedWith() && Utils.DoesArrayContainT<SwordItemData, ActiveItemData>(GameManager.Instance.playerData.EquipedItemsData);
    }
}
