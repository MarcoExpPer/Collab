using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Interfaces;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialoguePanel : MonoBehaviour, MainUIPanel
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI sourceNameText;
    [SerializeField] private RawImage sourceImage;
    [SerializeField] private DialogueChoiceButton[] _choicesButtons;
    
    public UnityEvent onDialogueFinished = new UnityEvent();
    
    private DialogueBase _currentDialogue;
    private DialogueStepData _currentStep;
    private int _currentDialogueIndex;
    private Coroutine _nextDialogueTimer;
    
    private void OnValidate()
    {
        _choicesButtons = GetComponentsInChildren<DialogueChoiceButton>();
    }

    private void Awake()
    {
        GameManager.Instance.uiManager.dialoguePanel = this;
        sourceNameText.enabled = false;
        sourceImage.enabled = false;

        gameObject.SetActive(false);
    }

    public void SetDialogueData(DialogueBase dialogueBase)
    {
        _currentDialogue = dialogueBase;
        _currentDialogueIndex = 0;
    }

    public void Toggle(bool setActive)
    {
        gameObject.SetActive(setActive);

        if (setActive)
        {
            GoToDialogueStep(_currentDialogueIndex);
        }
    }
    
    public void GoToDialogueStep(int index)
    {
        if (index >= _currentDialogue.DialogueSteps.Length)
        {
            EndDialogue();
            return;
        }

        _currentDialogueIndex = index;
        _currentStep = _currentDialogue.DialogueSteps[index];

        SetDialogueSource(_currentStep.DialogueSource);
        dialogueText.text = _currentStep.DialogueText;

        if (_currentStep.DialogueStepType == EDialogueStepType.withChoices)
        {
            SetDialogueChoices(_currentStep.ChoicesData);
        }
        else
        {
            CloseDialogueChoices();
            //If we dont select buttons, we need to select this panel to progress in the dialogue
            EventSystem.current.SetSelectedGameObject(gameObject, null);
        }

        if (_nextDialogueTimer != null)
        {
            StopCoroutine(_nextDialogueTimer);
        }

        if (_currentStep.AutoConfirmSecondsTimer > 0)
        {
            _nextDialogueTimer = StartCoroutine(AutoConfirmDialogue(_currentStep.AutoConfirmSecondsTimer));
        }
    }

    IEnumerator AutoConfirmDialogue(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        ConfirmInput(false);
    }

    public void SetDialogueSource(DialogueSourceData sourceData)
    {
        if (sourceData)
        {
            sourceNameText.text = sourceData.SourceName;
            sourceImage.texture = sourceData.SourceImage;

            sourceNameText.enabled = true;
            sourceImage.enabled = true;
        }
        else
        {
            sourceNameText.enabled = false;
            sourceImage.enabled = false;
        }
    }

    private void SetDialogueChoices(DialogueChoiceData[] choices)
    {
        for (int i = 0; i < choices.Length; i++)
        {
            DialogueChoiceData choice = choices[i];
            DialogueChoiceButton choiceButton = _choicesButtons[i];
            choiceButton.Setup(choice);

            if (i == 0)
            {
                EventSystem.current.SetSelectedGameObject(choiceButton.gameObject, null);
            }
        }
    }

    private void CloseDialogueChoices()
    {
        foreach (DialogueChoiceButton ChoiceBtn in _choicesButtons)
        {
            ChoiceBtn.gameObject.SetActive(false);
        }
    }

    private void EndDialogue()
    {
        sourceNameText.enabled = false;
        sourceImage.enabled = false;
        
        onDialogueFinished.Invoke();
        gameObject.SetActive(false);
    }

    public void ConfirmFromChoiceButton(DialogueChoiceData choiceSelected)
    {
        switch (choiceSelected.choiceType)
        {
            case EDialogueChoiceType.closeDialogue:
            {
                EndDialogue();
                break;
            }
            case EDialogueChoiceType.goToNextDialogue:
            {
                _currentDialogueIndex++;
                GoToDialogueStep(_currentDialogueIndex);
                break;
            }
            //TO BE DEFINED
            case EDialogueChoiceType.other:
            {
                break;
            }
            case EDialogueChoiceType.goToParamDialogue:
            {
                _currentDialogueIndex = (int) choiceSelected.OptionParameter;
                GoToDialogueStep(_currentDialogueIndex);
                break;
            }
        }
    }

    /**
     * INPUTS
     */
    public bool ConfirmInput(bool fromMouse)
    {
        switch (_currentStep.DialogueStepType)
        {
            case EDialogueStepType.withChoices:
            {
                if (fromMouse)
                {
                    DialogueChoiceButton buttonClicked = UIManager.GetSelectableUiUnderMouse<DialogueChoiceButton>();
                    if (buttonClicked)
                    {
                        ConfirmFromChoiceButton(buttonClicked.ChoiceData);
                        return true;
                    }
                }
                else
                {
                    if (EventSystem.current.currentSelectedGameObject)
                    {
                        DialogueChoiceButton buttonSelected = EventSystem.current.currentSelectedGameObject.GetComponent<DialogueChoiceButton>();
                        ConfirmFromChoiceButton(buttonSelected.ChoiceData);
                        return true;
                    }
                }

                break;
            }
            case EDialogueStepType.normal:
            {
                _currentDialogueIndex++;
                GoToDialogueStep(_currentDialogueIndex);
                break;
            }
            case EDialogueStepType.closeAfterDialogue:
            {
                EndDialogue();
                break;
            }
        }

        return true;
    }

    public bool ItemInput(EEquipedItemSlot itemSlot, bool fromMouse)
    {
        if (fromMouse)
        {
            return false;
        }
        else
        {
            return ConfirmInput(false);
        }
    }

    public bool IsSelectable()
    {
        return _currentStep.DialogueStepType != EDialogueStepType.withChoices;
    }
}