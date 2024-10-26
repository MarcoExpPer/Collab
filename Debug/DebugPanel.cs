
using Interfaces;
using UnityEngine;

public class DebugPanel : MonoBehaviour, MainUIPanel
{
    void Awake()
    {
        GameManager.Instance.uiManager.debugPanel = this;
        gameObject.SetActive(false);
    }
    
    public bool ConfirmInput(bool fromMouse)
    {
        if (fromMouse)
        {
            DebugButton btnHit = UIManager.GetSelectableUiUnderMouse<DebugButton>();
            if (btnHit == null)
            {
                return false;
            }
         
            btnHit.OnClick();
            return true;
        }
        return false;
    }

    public bool ItemInput(EEquipedItemSlot itemSlot, bool fromMouse)
    {
        //False so it fallbacks to the normal confirm input
        return false;
    }

    public void Toggle(bool setActive)
    {
        gameObject.SetActive(setActive);
    }
}
