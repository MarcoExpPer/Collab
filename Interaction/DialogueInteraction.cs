using System;
using System.Linq;
using UnityEngine;

public class DialogueInteraction : InteractionSource
{
    [SerializeField] private DialogueBase dialogueBase;
    [SerializeField] private bool canRepeatDialogue = true;
    
    public override void ExecuteInteraction(InteractionController controller)
    {
        GameManager.Instance.uiManager.dialoguePanel.SetDialogueData(dialogueBase);
        GameManager.Instance.uiManager.ActivePanels.Add(GameManager.Instance.uiManager.dialoguePanel);
        GameManager.Instance.playerSmController.StateMachine.ChangeState(GameManager.Instance.playerSmController.PlUserInterfaceState);
        
        if (!canRepeatDialogue)
        {
            Destroy(gameObject);
        }
    }
}
