using System;
using UnityEngine;

public class GateInteraction : InteractionSource
{
    private Animator animator;
    private bool closing = false;
    private bool opening = false;
    private bool hasKeyDoor = false;
    private bool opened = false;

    public override void Awake()
    {
        base.Awake();
        animator = transform.parent.GetComponent<Animator>();
        PlayerTrigger playerTrigger = transform.parent.GetComponent<PlayerTrigger>();
        playerTrigger.onPlayerEnterTrigger.AddListener(PlayerInside);
        playerTrigger.onPlayerExitTrigger.AddListener(PlayerExit);
    }

    public override void ExecuteInteraction(InteractionController controller)
    {
        controller.EndInteraction();
        animator.SetFloat("Direction", 1.0f);
        animator.SetBool("Opened", true);
        hasKeyDoor = true;
        opening = true;
        base.HideInteractionWidget();
    }

    public override bool CanBeInteractedWith()
    {
        if (!opening && !hasKeyDoor)
        {
            if (base.CanBeInteractedWith())
            {
                bool hasKeyDoor = GameManager.Instance.playerData.HasPassiveItem(EItemId.KeyDoor);
                return hasKeyDoor;
            }
        }
        return false;
    }

    void PlayerInside(GameObject obtainedBy)
    {
        if(closing)
        {
            closing = false;
            opening = true;
            animator.SetFloat("Direction", 1.0f);
        }
        else if(!closing && hasKeyDoor)
        {
            opening = true;
            animator.SetFloat("Direction", 1.0f);
            animator.SetBool("Opened", true);
        }

    }

    void PlayerExit(GameObject obtainedBy)
    {
        if(opening)
        {
            animator.SetFloat("Direction", -1.0f);
            closing = true;
            opening = false;
            opened = false;
        }
    }

    private void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if(stateInfo.IsName("OpenGate") && stateInfo.normalizedTime <= 0.0f)
        {
            animator.SetBool("Opened", false);
            opening = false;
            closing = false;
        }
        if(stateInfo.IsName("OpenGate") && stateInfo.normalizedTime >= 1.0f && !opened && !closing)
        {
            opened = true;
            animator.SetFloat("Direction", 0.0f);
        }
    }
}