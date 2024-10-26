using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Animator))]
public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDeselectHandler, ISelectHandler
{
    [SerializeField,HideInInspector] Animator animator;
    [SerializeField,HideInInspector] Selectable selectable;
    
    public bool mouseInside = false;

    public void OnValidate()
    {
        animator = GetComponent<Animator>();
        selectable = GetComponent<Selectable>();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseInside = true;
        if (!EventSystem.current.alreadySelecting)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseInside = false;
    }
    
    public void OnDeselect(BaseEventData eventData)
    {
        selectable.OnPointerExit(null);
        animator.SetBool("selected", false);
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        animator.SetBool("selected", true);
    }
}
