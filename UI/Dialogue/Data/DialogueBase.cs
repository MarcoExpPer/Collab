using System;
using Unity.VisualScripting;
using UnityEngine;
using VInspector;


public enum EDialogueChoiceType
{
    closeDialogue = 0,
    goToNextDialogue = 1,
    goToParamDialogue,
    other
}

public enum EDialogueStepType
{
    normal,
    withChoices,
    closeAfterDialogue
}

//These are classes because unity doesnt serialize Structs
[Serializable]
public class DialogueChoiceData
{
    public EDialogueChoiceType choiceType = EDialogueChoiceType.goToNextDialogue;
    public string OptionText;
    public float OptionParameter;
}

[Serializable]
public class DialogueStepData
{
    public DialogueSourceData DialogueSource;
    public string DialogueText;
    public float AutoConfirmSecondsTimer = 0;
    public EDialogueStepType DialogueStepType = EDialogueStepType.normal;
    public DialogueChoiceData[] ChoicesData;
}



[CreateAssetMenu(menuName = "Nilsh/Dialogue/DialogueBase")]
public class DialogueBase : ScriptableObject
{
    public DialogueStepData[] DialogueSteps;
}
