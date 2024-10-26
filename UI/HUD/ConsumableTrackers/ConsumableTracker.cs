
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public abstract class ConsumableTracker : MonoBehaviour
{
    public float timeToShowAfterLooting = 5f;
    protected Coroutine _hideCoroutine;

    [SerializeField] protected RawImage _rawImage;
    [SerializeField] protected TextMeshProUGUI _text;
    
    public void Awake()
    {
        _rawImage.enabled = false;
        _text.enabled = false;
        _text.text = "0";
    }

    public abstract void SetupPlayerData(PlayerData playerData);
    
    //This function ignores the coroutine. Is intended to be called from the consumible panel when opening/closing any menu
    public void ToggleView(bool show)
    {
        if (show)
        {
            ToggleEnables(true);
            
            if (_hideCoroutine != null)
            {
                StopCoroutine(_hideCoroutine);
            }
        }
        else
        {
            ToggleEnables(false);
        }
    }
    
    protected IEnumerator HideText()
    {
        yield return new WaitForSeconds(timeToShowAfterLooting);
        ToggleEnables(false);
    }

    protected void ToggleEnables(bool show)
    {
        _rawImage.enabled = show;
        _text.enabled = show;
    }

    public void OnValidate()
    {
        _rawImage = GetComponentInChildren<RawImage>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }
}
