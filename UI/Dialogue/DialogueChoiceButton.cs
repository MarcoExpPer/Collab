using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Interfaces;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class DialogueChoiceButton : MonoBehaviour, IPointerEnterHandler, IDeselectHandler
{
    [SerializeField] private Button _button;
    private DialoguePanel _dialoguePanel;
    public DialogueChoiceData ChoiceData {get; private set;}
    
    [SerializeField] private TextMeshProUGUI _text;
    private void OnValidate()
    {
        _button = GetComponent<Button>();
        _button.interactable = false;

        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        _dialoguePanel = GameManager.Instance.uiManager.dialoguePanel;
    }

    public void Setup(DialogueChoiceData choice)
    {
        ChoiceData = choice;
        _button.interactable = true;
        _text.text = choice.OptionText;
        
        gameObject.SetActive(true);
    }

    public void OnDisable()
    {
        _button.interactable = false;
    }

    /**
     * INPUTS
     */
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!EventSystem.current.alreadySelecting)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
    
    public void OnDeselect(BaseEventData eventData)
    {
        GetComponent<Selectable>().OnPointerExit(null);
    }
}
