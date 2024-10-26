
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;


public class UIManager : MonoBehaviour
{
    [HideInInspector] public InventoryPanel inventoryPanel;
    [HideInInspector] public EquipedItemsHUD equipedItemsHUD;
    [HideInInspector] public HUD_Panel hudPanel;
    [HideInInspector] public DialoguePanel dialoguePanel;
    [HideInInspector] public DeathScreen DeathScreen;
    [HideInInspector] public DebugPanel debugPanel;
    
    [HideInInspector] public List<MainUIPanel> ActivePanels = new List<MainUIPanel>();
    
    public static TReturnClass GetSelectableUiUnderMouse<TReturnClass>()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        if (results.Count > 0)
        {
            return results[0].gameObject.GetComponent<TReturnClass>();
        }

        return default;
    }
}
