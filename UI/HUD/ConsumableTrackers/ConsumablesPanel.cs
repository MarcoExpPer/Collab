
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class ConsumablesPanel : MonoBehaviour
{
    public float timeToShowAfterMenuClose = 5f;
        
    [SerializeField] [HideInInspector] private ConsumableTracker[] _consumableTrackers;
    private Coroutine _hideCoroutine;
    
    public void Start()
    {
        GameManager.Instance.uiManager.inventoryPanel.OnInventoryToggle.AddListener(ToggleView);
    }

    public void SetupPlayerData(PlayerData playerData)
    {
        foreach (var tracker in _consumableTrackers)
        {
            tracker.SetupPlayerData(playerData);
        }
    }
    
    public void ToggleView(bool show)
    {
        if (show)
        {
            if (_hideCoroutine != null)
            {
                StopCoroutine(_hideCoroutine);
            }

            foreach (var tracker in _consumableTrackers)
            {
                tracker.ToggleView(true);
            }
        }
        else
        {
            _hideCoroutine = StartCoroutine(HidePanel());
        }
    }

    IEnumerator HidePanel()
    {
        yield return new WaitForSeconds(timeToShowAfterMenuClose);

        foreach (var tracker in _consumableTrackers)
        {
            tracker.ToggleView(false);
        }
    }

    public void OnValidate()
    {
        _consumableTrackers = GetComponentsInChildren<ConsumableTracker>();
    }
}
